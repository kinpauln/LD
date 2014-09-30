using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using LotteryDraw.Component.Tools;
using System.Reflection.Emit;
using System.Collections;


namespace LotteryDraw.Component.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static void Update<TEntity, TKey>(this DbContext dbContext, params TEntity[] entities) where TEntity : EntityBase<TKey>
        {
            if (dbContext == null) throw new ArgumentNullException("dbContext");
            if (entities == null) throw new ArgumentNullException("entities");

            foreach (TEntity entity in entities)
            {
                DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
                try
                {
                    DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        dbSet.Attach(entity);
                        entry.State = EntityState.Modified;
                    }
                }
                catch (InvalidOperationException)
                {
                    TEntity oldEntity = dbSet.Find(entity.Id);
                    dbContext.Entry(oldEntity).CurrentValues.SetValues(entity);
                }
            }
        }

        public static void Update<TEntity, TKey>(this DbContext dbContext, Expression<Func<TEntity, object>> propertyExpression, params TEntity[] entities)
            where TEntity : EntityBase<TKey>
        {
            if (propertyExpression == null) throw new ArgumentNullException("propertyExpression");
            if (entities == null) throw new ArgumentNullException("entities");
            ReadOnlyCollection<MemberInfo> memberInfos = ((dynamic)propertyExpression.Body).Members;
            foreach (TEntity entity in entities)
            {
                DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
                try
                {
                    DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
                    entry.State = EntityState.Unchanged;
                    foreach (var memberInfo in memberInfos)
                    {
                        entry.Property(memberInfo.Name).IsModified = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    TEntity originalEntity = dbSet.Local.Single(m => Equals(m.Id, entity.Id));
                    ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
                    ObjectStateEntry objectEntry = objectContext.ObjectStateManager.GetObjectStateEntry(originalEntity);
                    objectEntry.ApplyCurrentValues(entity);
                    objectEntry.ChangeState(EntityState.Unchanged);
                    foreach (var memberInfo in memberInfos)
                    {
                        objectEntry.SetModifiedProperty(memberInfo.Name);
                    }
                }
            }
        }

        public static int SaveChanges(this DbContext dbContext, bool validateOnSaveEnabled)
        {
            bool isReturn = dbContext.Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
            try
            {
                dbContext.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return dbContext.SaveChanges();
            }
            finally
            {
                if (isReturn)
                {
                    dbContext.Configuration.ValidateOnSaveEnabled = !validateOnSaveEnabled;
                }
            }
        }

        public static IEnumerable SqlQueryForDynamic(this Database db,
                string sql,
                params object[] parameters)
        {
            IDbConnection defaultConn = new System.Data.SqlClient.SqlConnection();

            return SqlQueryForDynamicOtherDB(db, sql, defaultConn, parameters);
        }

        public static IEnumerable SqlQueryForDynamicOtherDB(this Database db,
                      string sql,
                      IDbConnection conn,
                      params object[] parameters)
        {
            conn.ConnectionString = db.Connection.ConnectionString;

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            IDataReader dataReader = cmd.ExecuteReader();

            if (!dataReader.Read())
            {
                return null; //无结果返回Null
            }

            #region 构建动态字段

            TypeBuilder builder = DbContextExtensions.CreateTypeBuilder(
                          "EF_DynamicModelAssembly",
                          "DynamicModule",
                          "DynamicType");

            int fieldCount = dataReader.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                //dic.Add(i, dataReader.GetName(i));

                //Type type = dataReader.GetFieldType(i);

                DbContextExtensions.CreateAutoImplementedProperty(
                  builder,
                  dataReader.GetName(i),
                  dataReader.GetFieldType(i));
            }

            #endregion

            dataReader.Close();
            dataReader.Dispose();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();

            Type returnType = builder.CreateType();

            if (parameters != null)
            {
                return db.SqlQuery(returnType, sql, parameters);
            }
            else
            {
                return db.SqlQuery(returnType, sql);
            }
        }

        public static TypeBuilder CreateTypeBuilder(string assemblyName,
                              string moduleName,
                              string typeName)
        {
            TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
              new AssemblyName(assemblyName),
              AssemblyBuilderAccess.Run).DefineDynamicModule(moduleName).DefineType(typeName,
              TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        public static void CreateAutoImplementedProperty(
                            TypeBuilder builder,
                            string propertyName,
                            Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
              string.Concat(
                PrivateFieldPrefix, propertyName),
              propertyType,
              FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
              propertyName,
              System.Reflection.PropertyAttributes.HasDefault,
              propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes = MethodAttributes.Public
              | MethodAttributes.SpecialName
              | MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(
                  GetterPrefix, propertyName),
                propertyMethodAttributes,
                propertyType,
                Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
              string.Concat(SetterPrefix, propertyName),
              propertyMethodAttributes,
              null,
              new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }
    }
}

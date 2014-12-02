// Դ�ļ�ͷ��Ϣ��
// <copyright file="AccountService.cs">
// Copyright(c)2014 Kingdon.All rights reserved.
// CLR�汾��4.0.30319.239
// ������֯��������@�й�
// ��˾��վ��http://www.wuliubang.net/
// �������̣�LotteryDraw.Core
// ����޸ģ�������
// ����޸ģ�2014/08/06 23:08
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using LotteryDraw.Component.Tools;
using LotteryDraw.Core.Data.Repositories.Account;
using LotteryDraw.Core.Data.Repositories.Security;
using LotteryDraw.Core.Models.Account;
using LotteryDraw.Core.Models.Security;
using LotteryDraw.Core.Data.Repositories.Business;
using LotteryDraw.Core.Models.Business;
using System.Data.SqlClient;
using System.Data;


namespace LotteryDraw.Core.Impl
{
    /// <summary>
    ///     �˻�ģ�����ҵ��ʵ��
    /// </summary>
    [Export(typeof(IPrizePhotoContract))]
    public class PrizePhotoService : CoreServiceBase, IPrizePhotoContract
    {
        #region ����

        #region �ܱ���������

        /// <summary>
        /// ��ȡ������ ��Ʒ���ݷ��ʶ���
        /// </summary>
        [Import]
        protected IPrizePhotoRepository PrizePhotoRepository { get; set; }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ �û���Ϣ��ѯ���ݼ�
        /// </summary>
        public IQueryable<PrizePhoto> PrizePhotos
        {
            get { return PrizePhotoRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        ///     ��ӽ�Ʒ
        /// </summary>
        /// <param name="prizebetting">��Ʒ��Ϣ</param>
        /// <returns>ҵ��������</returns>
        public OperationResult Add(PrizePhoto prizephoto)
        {
            int rcount = PrizePhotoRepository.Insert(prizephoto);
            if (rcount > 0)
            {
                return new OperationResult(OperationResultType.Success, "��ӽ�ƷͼƬ�ɹ���", prizephoto);
            }
            else
            {
                return new OperationResult(OperationResultType.Warning, "��ӽ�ƷͼƬʧ�ܡ�");
            }
        }
    }
}
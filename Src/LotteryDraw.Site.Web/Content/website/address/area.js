function showLocation(province , city , town) {
	
	var loc	= new Location();
	var title	= ['省份' , '地级市' , '市、县、区'];
	$.each(title , function(k , v) {
		title[k]	= '<option value="">'+v+'</option>';
	})

	$("select[name='loc_province']").append(title[0]);
	$("select[name='loc_city']").append(title[1]);
	$("select[name='loc_town']").append(title[2]);
	
	
	$("select[name='loc_province']").change(function () {
	    var $provice = $(this);
	    var $city = $provice.next("select");
	    var $town = $city.next("select");
	    $city.empty();
	    $city.append(title[1]);
	    loc.fillOption('loc_city', '0,' + $provice.val());
	    $town.empty();
	    $town.append(title[2]);
		//$('input[@name=location_id]').val($(this).val());
	})
	
	$("select[name='loc_city']").change(function () {
	    var $city = $(this);
	    var $provice = $city.prev("select");
	    var $town = $city.next("select");
	    $town.empty();
	    $town.append(title[2]);
	    loc.fillOption('loc_town', '0,' + $provice.val() + ',' + $city.val());
		//$('input[@name=location_id]').val($(this).val());
	})
	
	$("select[name='loc_town']").change(function () {
	    var $hidn = $(this).next("input[name='location_id']");
	    $hidn.val($(this).val());
	})
	
	if (province) {
		loc.fillOption('loc_province' , '0' , province);
		
		if (city) {
			loc.fillOption('loc_city' , '0,'+province , city);
			
			if (town) {
				loc.fillOption('loc_town' , '0,'+province+','+city , town);
			}
		}
		
	} else {
		loc.fillOption('loc_province' , '0');
	}
		
}
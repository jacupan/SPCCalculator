/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:24 AM
 * @description :   for easy hiding of element
 */
var t_strip = {
    hide: function () {
        $("#welcomeNote").hide();
        $("#tab1SpcMoldGts").hide();
        $("#tab1SpcMoldGts2").hide();
        $("#tab1SpcMoldSensor").hide();
        $("#tab1SpcMoldKMatrixTOKTKN").hide();
        $("#tab1SpcMoldKMatrixKEKFKT").hide();
        $("#tab1SpcMoldCS").hide();
        $("#tab1SpcMoldSohed").hide();
        $("#tab1SpcWbSohed").hide();
        $("#tab1SpcWbGts").hide();
        $("#tab1SpcWbSensor").hide();
        $("#tab1SpcWbSL").hide();

        $("#gridMoldGTSList").hide();
        $("#gridMoldGTS").hide();
        $("#gridMoldGTS2").hide();
        $("#gridMoldSENSOR").hide();
        $("#gridMoldKMATRIXTOKTKN").hide();
        $("#gridMoldKMATRIXKEKFKT").hide();
        $("#gridMoldCS").hide();
        $("#gridMoldSohed").hide();
        $("#gridWbSohedBs").hide();
        $("#gridWbSohedWp").hide();
        $("#gridWbGtsBs").hide();
        $("#gridWbGtsWp").hide();
        $("#gridWbSensorBs").hide();
        $("#gridWbSensorWp").hide();
        $("#gridWbSLBs").hide();
        $("#gridWbSLWp").hide();


        $("#tab_sensor_kektsipka").hide();
        $("#tab_ll").hide();
        $("#tab_monitoring_epin_only").hide();
        $("#tab_da_bondline").hide();
    },
    show: function (str1, str2) {
        var elem_id = '';

        t_strip.hide();

        switch (str1) {
            case "mold":

                switch (str2) {
                    case "gts":
                        elem_id = 'tab1SpcMoldGts';
                        break;
                    case "gts2":
                        elem_id = 'tab1SpcMoldGts2';
                        break;
                    case "ua matrix":
                        elem_id = 'tab1SpcMoldSensor';
                        break;
                    case "k-matrix tok/kb/kc/kn":
                        elem_id = 'tab1SpcMoldKMatrixTOKTKN';
                        break;
                    case "current sensor":
                        elem_id = 'tab1SpcMoldCS';
                        break;
                    case "sohed":
                        elem_id = 'tab1SpcMoldSohed';
                        break;
                    case "sensor (ke_kt_ua-sip_ka)":
                        elem_id = 'tab_sensor_kektsipka';
                        _load.dropdown('sensor_kektsipka');
                        break;
                    case "ll":
                        elem_id = 'tab_ll';
                        _load.dropdown('ll');
                        break;
                    case "100%monitoring_epin_only":
                        elem_id = 'tab_monitoring_epin_only';
                        break;
                    default:
                        elem_id = 'welcomeNote';
                }

                break;
            case "wirebond":

                switch (str2) {
                    case "gts":
                        elem_id = 'tab1SpcWbGts';
                        break;
                    case "sohed":
                        elem_id = 'tab1SpcWbSohed';
                        break;
                    case "sensor":
                        elem_id = 'tab1SpcWbSensor';
                        break;
                    case "sl":
                        elem_id = 'tab1SpcWbSL';
                        break;
                    default:
                        elem_id = 'welcomeNote';
                }

                break;
            case "die attach":

                switch (str2) {
                    case "bondline":
                        elem_id = 'tab_da_bondline';
                        //_load.dropdown('da_bondline');
                        break;
                    default:
                        elem_id = 'welcomeNote';
                }

                break;
            default:
                elem_id = 'welcomeNote';
        }
        $("#"+elem_id).show();
    },
    test: function (str1, str2) {
        alert(str1 + str2);
    }
};

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:25 AM
 * @description :   capture the onClick event of the LOAD button
 */
$(document).delegate('.cmdLoader', 'click', function () {
    var category = $(this).parent().attr('data-category');
    var validation_result = validator.check(category);

    //validate if all necessary fields
    if(validation_result==true){
          $.ajax({
            url: '/Sensor/validateLotAndUser/',
            type: 'GET',
            dataType: 'json',
            data: {
                LotNbr : $("#txtLotNbr_"+category).val(),
                Username : $("#txtOperator_"+category).val(),
            },
            contentType: 'application/json',
            beforeSend: function(){
                _logs.clear(category);
            },
            success: function (response) {
                //console.log(response[0]);
                if(response[0]!='' && response[1]!=''){
                    _load.grid(category,response[0],response[1]);
                }else{
                    if (response[0] == '') {
                        //_logs.show(category,'err','Invalid lot #. Please try again.');
                        notification_modal('Unable to proceed!', 'Invalid Lot #.', 'danger');
                    } else {
                        //_logs.show(category,'err','Invalid user. Please try again.');
                        notification_modal('Unable to proceed!', 'Invalid User.', 'danger');
                    }
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }

        });
    }else{
        notification_modal('Unable to proceed!','Please provide the following:<br />'+validation_result+'<br />','danger');
        //_logs.show(category,'err','Please provide the following:<br />'+validation_result+'<br />');
    }

});

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:26 AM
 * @description :   capture the onClick event of the SAVE button in the grid
 */
$(document).delegate('.cmdSave', 'click', function () {
    //log for debugging
    //console.log($(this).attr('data-category'));
    //console.log(prep_data.for_saving($(this).attr('data-category')));

    //call the save function
    if(prep_data.for_saving($(this).attr('data-category'))!=false){
        if($(this).attr('data-category')=='da_bondline')
        {
            dbase.save(prep_data.for_saving($(this).attr('data-category')),$(this).attr('data-category'));
        }
        else
        {
            dbase.save(prep_data.for_saving($(this).attr('data-category')),$(this).attr('data-category'));
        }
        
        //alert('saving');
    }else{
        alert('Cannot proceed! : Empty values found on the grid!');
    }
});

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:28 AM
 * @description :   capture the onClick event of the VIEW DATA FOR CAMSTAR RECORDING button in the grid
 */
$(document).delegate('.cmdViewData', 'click', function () {
    dbase.viewData($(this).attr('data-category'));
});

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:29 AM
 * @description :   load object
 *              :   the following code is chaotic, if you have some better ideas please feel free to modify the code, we need to make a generic object in creating a grid
 */
var _load = {
    grid: function (ops,device,username) {

    var style_status = "text-align: center; font-weight:bold; font-size:small; background-color: LimeGreen";
    var style_data_from_camstar = "text-align: center; font-weight:bold; font-size:small; background-color: LightGray";
    var style_editable_fields = "text-align: center; font-weight:bold; font-size:small; background-color: Violet";
    var style_calculated_fields = "text-align: center; font-weight:bold; font-size:small; background-color: LightBlue";
    var style_header = "text-align: center; font-weight:bold; height:35px;";

        //check what grid is going to be loaded
        switch (ops) {
            case "sensor_kektsipka":
                $("#grid_sensor_kektsipka").kendoGrid({
                    dataSource: {
                        data: prep_data.generate(ops,device,username),  //prepare local data for the grid
                        schema: {
                            model: {
                                fields: {
                                    Lot: { type: "string", editable: false },
                                    Device: { type: "string", editable: false },
                                    FramePosition: { type: "string", editable: false },
                                    Machine: { type: "string", editable: false },
                                    Operator: { type: "string", editable: false },
                                    Remarks: { type: "string", editable: true },
                                    HEDA: { type: "number", format:"{0:N4}", editable: true },
                                    HEDB: { type: "number", format:"{0:N4}", editable: true },
                                    HEDC: { type: "number", format:"{0:N4}" },
                                    PackageWidth: { type: "number", format:"{0:N4}" },
                                    PackageHeight: { type: "number", format:"{0:N4}", editable: true },
                                    Epin: { type: "number", format:"{0:N4}", editable: true },
                                    Status: { type: "string", editable: false },
                                    PackageGroup: { type: "string", editable: false },
                                },
                            },
                        },
                    },
                    height: 550,
                    editable  : true,
                    navigatable: true,
                    resizable: true,
                    save : function (e) {
                        if (e.values && (e.values.HEDA || e.values.HEDB)) {
                            var a = e.values.HEDA || e.model.HEDA;
                            var b = e.values.HEDB || e.model.HEDB;

                            //changed as per Deo Vincent Calim
                            //e.model.set('HEDC', ((a-b)/2).toFixed(4));
                            e.model.set('HEDC', ((b-a)/2).toFixed(4));

                            e.model.set('PackageWidth', (a+b).toFixed(4));
                        }
                    },
                    columns: [{
                        field: "Lot",
                        title: "Lot",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "Device",
                        title: "Device",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "FramePosition",
                        title: "Frame Position",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "Machine",
                        title: "Machine",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    },{
                        field: "Operator",
                        title: "Operator",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    },{
                        field: "Remarks",
                        title: "Remarks",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_editable_fields
                        }
                    },{
                        field: "HEDA",
                        title: "HED-A",
                        editor: numberEditor,
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_editable_fields
                        }
                    },{
                        field: "HEDB",
                        title: "HED-B",
                        editor: numberEditor,
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_editable_fields
                        }
                    },{
                        field: "HEDC",
                        title: "HED-C",
                        editor: lockedNumberEditor,
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_calculated_fields
                        }
                    },{
                        field: "PackageWidth",
                        title: "Package Width",
                        editor: lockedNumberEditor,
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_calculated_fields
                        }
                    },{
                        field: "PackageHeight",
                        title: "Package Height",
                        editor: numberEditor,
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_editable_fields
                        }
                    },{
                        field: "Epin",
                        title: "E-PIN",
                        editor: numberEditor,
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_editable_fields
                        }
                    },{
                        field: "Status",
                        title: "Status",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_status
                        }
                    },{
                        field: "PackageGroup",
                        title: "Package Group",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    },],
                    toolbar: [{
                        template: '<input type="button" class="k-button k-primary cmdSave" data-category="sensor_kektsipka" value="Save"><input type="button" class="k-button k-primary cmdViewData" data-category="sensor_kektsipka" value="View data for CAMSTAR recording">',
                    }],
                });
                break;
            case "ll":
                var grid = $("#grid_ll").kendoGrid({
                    dataSource: {
                        data: prep_data.generate(ops,device,username),  //prepare local data for the grid
                        schema: {
                            model: {
                                fields: {
                                    Lot: { type: "string", editable: false },
                                    Device: { type: "string", editable: false },
                                    FramePosition: { type: "string", editable: false },
                                    Machine: { type: "string", editable: false },
                                    Operator: { type: "string", editable: false },
                                    Remarks: { type: "string", editable: true },
                                    HEDA: { type: "number", format:"{0:N4}", editable: true },
                                    HEDB: { type: "number", format:"{0:N4}", editable: true },
                                    HEDC: { type: "number", format:"{0:N4}" },
                                    Eartab: { type: "number", format:"{0:N4}", editable: true },
                                    Status: { type: "string", editable: false },
                                    PackageGroup: { type: "string", editable: false },
                                },
                            },
                        },
                    },
                    height: 550,
                    editable  : true,
                    navigatable: true,
                    resizable: true,
                    save : function (e) {
                        if (e.values && (e.values.HEDA || e.values.HEDB)) {
                            var a = e.values.HEDA || e.model.HEDA;
                            var b = e.values.HEDB || e.model.HEDB;

                            //changed as per Deo Vincent Calim
                            //e.model.set('HEDC', ((a-b)/2).toFixed(4));
                            e.model.set('HEDC', ((b-a)/2).toFixed(4));
                        }
                    },
                    columns: [{
                        field: "Lot",
                        title: "Lot",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "Device",
                        title: "Device",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "FramePosition",
                        title: "Frame Position",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "Machine",
                        title: "Machine",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    },{
                        field: "Operator",
                        title: "Operator",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    },{
                        field: "Remarks",
                        title: "Remarks",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_editable_fields
                        }
                    },{
                        field: "HEDA",
                        title: "HED-A",
                        editor: ($("#cmbPackageGroup_"+ops).data("kendoDropDownList").text()=='EarTab' ? lockedNumberEditor : numberEditor),
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : ($("#cmbPackageGroup_"+ops).data("kendoDropDownList").text()=='EarTab' ? style_calculated_fields : style_editable_fields)
                        }
                    },{
                        field: "HEDB",
                        title: "HED-B",
                        editor: ($("#cmbPackageGroup_"+ops).data("kendoDropDownList").text()=='EarTab' ? lockedNumberEditor : numberEditor),
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : ($("#cmbPackageGroup_"+ops).data("kendoDropDownList").text()=='EarTab' ? style_calculated_fields : style_editable_fields)
                        }
                    },{
                        field: "HEDC",
                        title: "HED-C",
                        editor: lockedNumberEditor,
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_calculated_fields
                        }
                    },{
                        field: "Eartab",
                        title: "EarTab",
                        editor: ($("#cmbPackageGroup_"+ops).data("kendoDropDownList").text()=='EarTab' ? numberEditor : lockedNumberEditor),
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : ($("#cmbPackageGroup_"+ops).data("kendoDropDownList").text()=='EarTab' ? style_editable_fields : style_calculated_fields)
                        }
                    },{
                        field: "Status",
                        title: "Status",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_status
                        }
                    },{
                        field: "PackageGroup",
                        title: "Package Group",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    },],
                    toolbar: [{
                        template: '<input type="button" class="k-button k-primary cmdSave" data-category="ll" value="Save"><input type="button" class="k-button k-primary cmdViewData" data-category="ll" value="View data for CAMSTAR recording">',
                    }],
                }).data("kendoGrid");

                /*if($("#cmbPackageGroup_"+ops).data("kendoDropDownList").text()=='EarTab'){
                    grid.hideColumn("HEDA");
                }else{
                    grid.showColumn("HEDA");
                }*/

                break;
            case "epin":
                $("#grid_epin").kendoGrid({
                    dataSource: {
                        data: prep_data.generate(ops,device,username),  //prepare local data for the grid
                        schema: {
                            model: {
                                fields: {
                                    Lot: { type: "string", editable: false },
                                    Device: { type: "string", editable: false },
                                    FramePosition: { type: "string", editable: false },
                                    Machine: { type: "string", editable: false },
                                    Operator: { type: "string", editable: false },
                                    //Remarks: { type: "string", editable: true },
                                    Epin: { type: "number", format:"{0:N4}", editable: true },
                                    Status: { type: "string", editable: false },
                                },
                            },
                        },
                    },
                    height: 550,
                    editable  : true,
                    navigatable: true,
                    resizable: true,
//                    save : function (e) {
//                        if (e.values && (e.values.HEDA || e.values.HEDB)) {
//                            var a = e.values.HEDA || e.model.HEDA;
//                            var b = e.values.HEDB || e.model.HEDB;
//                            e.model.set('HEDC', ((a-b)/2).toFixed(4));
//                        }
//                    },
                    columns: [{
                        field: "Lot",
                        title: "Lot",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "Device",
                        title: "Device",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "FramePosition",
                        title: "Frame Position",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    }, {
                        field: "Machine",
                        title: "Machine",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    },{
                        field: "Operator",
                        title: "Operator",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_data_from_camstar
                        }
                    },/*{
                        field: "Remarks",
                        title: "Remarks",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_editable_fields
                        }
                    },*/{
                        field: "Epin",
                        title: "EPIN",
                        editor: numberEditor,
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_editable_fields
                        }
                    },{
                        field: "Status",
                        title: "Status",
                        headerAttributes:{
                            "style" : style_header
                        },
                        attributes: {
                            "style" : style_status
                        }
                    },],
                    toolbar: [{
                        template: '<input type="button" class="k-button k-primary cmdSave" data-category="epin" value="Save">',
                    }],
                });
                break;
            case "da_bondline":
                    $("#grid_da_bondline").kendoGrid({
                        dataSource: {
                            data: prep_data.generate(ops, device, username),  //prepare local data for the grid
                            schema: {
                                model: {
                                    fields: {
                                        Lot: { type: "string", editable: false },
                                        Device: { type: "string", editable: false },
                                        Machine: { type: "string", editable: false },
                                        Operator: { type: "string", editable: false },
                                        Remarks: { type: "string", editable: false },

                                        Z1_w_epoxy: { type: "number", format: "{0:N4}", editable: true },
                                        Z2_w_epoxy: { type: "number", format: "{0:N4}", editable: true },
                                        Z3_w_epoxy: { type: "number", format: "{0:N4}", editable: true },

                                        Z1_wo_epoxy: { type: "number", format: "{0:N4}", editable: true },
                                        Z2_wo_epoxy: { type: "number", format: "{0:N4}", editable: true },
                                        Z3_wo_epoxy: { type: "number", format: "{0:N4}", editable: true },

                                        Z_average_wo: { type: "number", format: "{0:N4}" },
                                        BLT1: { type: "number", format: "{0:N4}" },
                                        BLT2: { type: "number", format: "{0:N4}" },
                                        BLT3: { type: "number", format: "{0:N4}" },

                                    },
                                },
                            },
                        },
                        height: 550,
                        editable: true,
                        navigatable: true,
                        resizable: true,
                        save: function (e) {
                            if (e.values && (e.values.Z1_wo_epoxy || e.values.Z2_wo_epoxy || e.values.Z3_wo_epoxy)) {
                                var a = e.values.Z1_wo_epoxy || e.model.Z1_wo_epoxy;
                                var b = e.values.Z2_wo_epoxy || e.model.Z2_wo_epoxy;
                                var c = e.values.Z3_wo_epoxy || e.model.Z3_wo_epoxy;
                                var av = ((a + b + c) / 3).toFixed(4);

                                e.model.set('Z_average_wo', av);

                                var z1 = e.values.Z1_w_epoxy || e.model.Z1_w_epoxy;
                                e.model.set('BLT1', (z1 - av).toFixed(4));
                                var z2 = e.values.Z2_w_epoxy || e.model.Z2_w_epoxy;
                                e.model.set('BLT2', (z2 - av).toFixed(4));
                                var z3 = e.values.Z3_w_epoxy || e.model.Z3_w_epoxy;
                                e.model.set('BLT3', (z3 - av).toFixed(4));
                            }
                        },
                        columns: [{
                            field: "Lot",
                            title: "Lot",
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_data_from_camstar
                            }
                        }, {
                            field: "Device",
                            title: "Device",
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_data_from_camstar
                            }
                        }, {
                            field: "Machine",
                            title: "Machine",
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_data_from_camstar
                            }
                        }, {
                            field: "Operator",
                            title: "Operator",
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_data_from_camstar
                            }
                        }, {
                            field: "Remarks",
                            title: "Remarks",
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_data_from_camstar
                            }
                        }, {
                            field: "Z1_w_epoxy",
                            title: "Z1 w/ Epoxy",
                            editor: numberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_editable_fields
                            }
                        }, {
                            field: "Z2_w_epoxy",
                            title: "Z2 w/ Epoxy",
                            editor: numberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_editable_fields
                            }
                        }, {
                            field: "Z3_w_epoxy",
                            title: "Z3 w/ Epoxy",
                            editor: numberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_editable_fields
                            }
                        }, {
                            field: "Z1_wo_epoxy",
                            title: "Z1 WITHOUT Epoxy",
                            editor: numberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_editable_fields
                            }
                        }, {
                            field: "Z2_wo_epoxy",
                            title: "Z2 WITHOUT Epoxy",
                            editor: numberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_editable_fields
                            }
                        }, {
                            field: "Z3_wo_epoxy",
                            title: "Z3 WITHOUT Epoxy",
                            editor: numberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_editable_fields
                            }
                        }, {
                            field: "Z_average_wo",
                            title: "Z Average (WITHOUT)",
                            editor: lockedNumberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_calculated_fields
                            }
                        }, {
                            field: "BLT1",
                            title: "BLT1",
                            editor: lockedNumberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_calculated_fields
                            }
                        }, {
                            field: "BLT2",
                            title: "BLT2",
                            editor: lockedNumberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_calculated_fields
                            }
                        }, {
                            field: "BLT3",
                            title: "BLT3",
                            editor: lockedNumberEditor,
                            headerAttributes: {
                                "style": style_header
                            },
                            attributes: {
                                "style": style_calculated_fields
                            }
                        }],
                        toolbar: [{
                            template: '<input type="button" class="k-button k-primary cmdSave" data-category="da_bondline" value="Save"><input type="button" class="k-button k-primary cmdIntegrateData" data-category="da_bondline" value="Integrate to CAMSTAR">',
                        }],
                    });
                    break;
        }
    },
    //static dropdown values
    dropdown: function(ops){
        switch (ops) {
            case "sensor_kektsipka":
                var data = [
                        {text: "KE", value:"KE"},
                        {text: "KT", value:"KT"},
                        {text: "UA-SIP", value:"UA-SIP"},
                        {text: "KA", value:"KA"},
                    ];
            break;
            case "ll":
                var data = [
                        {text: "EarTab", value:"EarTab"},
                        {text: "PTFM", value:"PTFM"},
                    ];
            break;
        }

        $("#cmbPackageGroup_"+ops).kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: data,
        });
    },
    dropdownlist: function(){
        $(".cmbTest").kendoDropDownList({
            dataTextField: "ResourceName",
            dataValueField: "ResourceId",
            width: '100%',
            dataSource:
            {
                transport:
                {
                    read:
                    {
                        dataType: "json",
                        url: "Home/MachineListSensorKEKTSIPKA",
                    }
                }
            }
        });
    },
};

//custom number editor for the kendogrid
function numberEditor(container, options) {
    $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                format  : "{0:n4}",
                decimals: 4,
                spinners: false,
                //step    : 0.001
            });
}

//locked number editor for the kendogrid
function lockedNumberEditor(container, options) {
    $('<input name="' + options.field + '" readonly="readonly"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                format  : "{0:n4}",
                decimals: 4,
                spinners: false
                //step    : 0.001
            });
}

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:29 AM
 * @description :   prep_data object - for initializing json data to be used by the grid
 *              :   this is also chaotic, we can just declare the default number of lines per category then make a loop
 */
var prep_data = {
    generate: function (item,device,username) {

        var json_data = [];
        switch (item) {
            case "sensor_kektsipka":
                var lot_nbr = $("#txtLotNbr_"+item).val();
                var machine = $("#cmbMachine_"+item).data("kendoDropDownList").text();
                //var operator = $("#txtOperator_"+item).val();
                var frame_position = $("#txtFramePos_"+item).val();
                var package_group = $("#cmbPackageGroup_"+item).data("kendoDropDownList").text();

                var sample_data = {
                            Lot: lot_nbr,
                            Device: device,
                            FramePosition: frame_position,
                            Machine: machine,
                            Operator: username,
                            Remarks: "",
                            HEDA: '',
                            HEDB: '',
                            HEDC: '',
                            PackageWidth: '',
                            PackageHeight: '',
                            Epin: '',
                            Status: "NEW",
                            PackageGroup: package_group
                            };

                var json_data = [];
                for(var i = 0; i < 3; i++){
                    json_data.push(sample_data);
                }

            break;
            case "ll":
                var lot_nbr = $("#txtLotNbr_"+item).val();
                var machine = $("#cmbMachine_"+item).data("kendoDropDownList").text();
                //var operator = $("#txtOperator_"+item).val();
                var frame_position = $("#txtFramePos_"+item).val();
                var package_group = $("#cmbPackageGroup_"+item).data("kendoDropDownList").text();

                var ctr = 3;
                if(package_group=='EarTab'){
                    ctr = 5;
                }
                var sample_data = {
                            Lot: lot_nbr,
                            Device: device,
                            FramePosition: frame_position,
                            Machine: machine,
                            Operator: username,
                            Remarks: "",
                            HEDA: '',
                            HEDB: '',
                            HEDC: '',
                            Eartab: '',
                            Status: "NEW",
                            PackageGroup: package_group
                            };

                var json_data = [];
                for(var i = 0; i < ctr; i++){
                    json_data.push(sample_data);
                }

            break;
            case "epin":
                var lot_nbr = $("#txtLotNbr_"+item).val();
                var machine = $("#cmbMachine_"+item).data("kendoDropDownList").text();
                //var operator = $("#txtOperator_"+item).val();
                var frame_position = $("#txtFramePos_"+item).val();

                var sample_data = {
                            Lot: lot_nbr,
                            Device: device,
                            FramePosition: frame_position,
                            Machine: machine,
                            Operator: username,
                            //Remarks: "",
                            Epin: '',
                            Status: "NEW",
                            };

                var json_data = [];
                for(var i = 0; i < 3; i++){
                    json_data.push(sample_data);
                }

                break;
            case "da_bondline":
                    var lot_nbr = $("#txtLotNbr_" + item).val();
                    var machine = $("#cmbMachine_" + item).data("kendoDropDownList").text();
                    var remarks = $("#txtRemarks_" + item).val();

                    var sample_data = {
                        Lot: lot_nbr,
                        Device: device,
                        FramePosition: frame_position,
                        Machine: machine,
                        Operator: username,
                        Remarks: remarks,
                        Z1_w_epoxy: '',
                        Z2_w_epoxy: '',
                        Z3_w_epoxy: '',
                        Z1_wo_epoxy: '',
                        Z2_wo_epoxy: '',
                        Z3_wo_epoxy: '',
                        Z_average_wo: '',
                        BLT1: '',
                        BLT2: '',
                        BLT3: '',
                    };

                    var json_data = [];
                    for (var i = 0; i < 2; i++) {
                        json_data.push(sample_data);
                    }

                    break;
        }

        return json_data;
    },
    for_saving: function(ops){
        var str = '';
        var arr = [];
        var must_not_be_empty = [];
        var log_empty = '';

        if(ops=='sensor_kektsipka'){
            arr = ['Lot','Device','FramePosition','Machine','Operator','Remarks','HEDA','HEDB','HEDC','PackageWidth','PackageHeight','Epin','Status','PackageGroup'];
            must_not_be_empty = [5,6,7,10,11];
        }else if(ops=='ll'){
            arr = ['Lot','Device','FramePosition','Machine','Operator','Remarks','HEDA','HEDB','HEDC','Eartab','Status','PackageGroup'];
            must_not_be_empty = [5,6,7,9];
            //must_not_be_empty = [6,7];
        } else if (ops == 'epin') {
            //arr = ['Lot','Device','FramePosition','Machine','Operator','Remarks','Epin','Status'];
            arr = ['Lot', 'Device', 'FramePosition', 'Machine', 'Operator', 'Epin', 'Status'];
            must_not_be_empty = [5, 6];
        } else if (ops == 'da_bondline') {
            arr = ['Lot', 'Device', 'Machine', 'Operator', 'Remarks', 'Z1_w_epoxy', 'Z2_w_epoxy', 'Z3_w_epoxy', 'Z1_wo_epoxy', 'Z2_wo_epoxy', 'Z3_wo_epoxy', 'Z_average_wo', 'BLT1', 'BLT2', 'BLT3'];
            must_not_be_empty = [5, 6, 7, 8, 9, 10];
        }

        // Gets the data source from the grid.
        var dataSource = $("#grid_"+ops).data("kendoGrid").dataSource;

        // Gets the filter from the dataSource
        var filters = dataSource.filter();

        // Gets the full set of data from the data source
        var allData = dataSource.data();

        // Applies the filter to the data
        var query = new kendo.data.Query(allData);
        var filteredData = query.filter(filters).data;

        str = '';
        var empty_checker = '';
        $.each(filteredData, function(index, item){
            for(var i=0; i< arr.length; i++){

                //if(empty_checker != -1 && (item[arr[i]]==null || item[arr[i]]=='null' || item[arr[i]]=='' || item[arr[i]].toString.length==0)){
                if(ops=='ll'){
                    if($("#cmbPackageGroup_"+ops).data("kendoDropDownList").text()=='EarTab'){
                        must_not_be_empty = [6,7,8];
                        if(must_not_be_empty.indexOf(i) != -1){
                            str += '0^';
                        }else{
                            str += item[arr[i]]+'^';
                        }
                    }else{
                        must_not_be_empty = [9];
                        if(must_not_be_empty.indexOf(i) != -1){
                            str += '0^';
                        }else{
                            str += item[arr[i]]+'^';
                        }
                    }
                }else{
                    str += item[arr[i]]+'^';
                }

            }
            str = str.substr(0, str.length - 1);
            str += '|';
        });
        str = str.substr(0, str.length - 1);

        console.log(str);
        //if(log_empty=='' && (str.indexOf("^^") == -1) && (str.indexOf("null") == -1)){
        if( str.indexOf("^^") == -1 && str.indexOf("null") == -1){
            return str;
        }else{
            return false;
        }

    },
};

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:29 AM
 * @description :   validator object
 *              :   this must be modified to cater other validations
 */
var validator = {
    check: function (ops) {
        var missing = '';

        //        if(ops=='sensor_kektsipka' || ops=='ll'){
        if (typeof $("#txtLotNbr_" + ops) != 'undefined' && $("#txtLotNbr_" + ops).val() == '') {
            missing += '<br />Lot #';
        }

        if (typeof $("#cmbMachine_" + ops) != 'undefined' && $("#cmbMachine_" + ops).data("kendoDropDownList").text() == '') {
            missing += '<br />Machine';
        }

        if (typeof $("#txtOperator_" + ops) != 'undefined' && $("#txtOperator_" + ops).val() == '') {
            missing += '<br />Operator';
        }

        if (typeof $("#txtFramePos_" + ops) != 'undefined' && $("#txtFramePos_" + ops).val() == '') {
            missing += '<br />Frame Position';
        }

        if (typeof $("#txtRemarks_" + ops) != 'undefined' && $("#txtRemarks_" + ops).val() == '') {
            missing += '<br />Remarks';
        }

        if (missing != '') {
            return missing;
        } else {
            return true;
        }
        //        }else{
        //            return false;
        //        }
    }
};

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:29 AM
 * @description :   database object
 */
var dbase = {
    save: function (strData, module) {
        var url = "";

        if (module == 'da_bondline') {
            url = '/DABondline/AddRecord/';
        } else {
            url = '/Sensor/AddRecord/';
        }

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                _data: strData,
                _module: module
            },
            contentType: 'application/json',
            success: function (response) {
                if (response) {
                    //alert('Saving successful!');
                    _logs.show(module, 'success', 'Saving successful!');

                    if (module == 'da_bondline') {
                        $("#grid_da_bondline").data("kendoGrid").dataSource.data([]);
                    } else {
                        if (module == 'sensor_kektsipka' || module == 'kektsipka') {
                            $("#grid_sensor_kektsipka").data("kendoGrid").dataSource.data([]);
                        } else if (module == 'll') {
                            $("#grid_ll").data("kendoGrid").dataSource.data([]);
                        } else {
                            $("#grid_epin").data("kendoGrid").dataSource.data([]);
                        }
                    }

                } else {
                    //alert("Saving failed!");
                    _logs.show(module, 'err', 'Saving failed!');
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }

        });
    },
    viewData: function(ops){
        var module = '';
        if(ops=='sensor_kektsipka'){
            module = 'kektsipka'
        }else if(ops=='ll'){
            module = 'll'
        }else{
            module = 'epin'
        }

        $.ajax({
            url: '/Sensor/GetDataForCamstarRecording/',
            type: 'GET',
            data: {
                LotNbr : $("#txtLotNbr_"+ops).val(),
                Module: module,
                PackageGroup : $("#cmbPackageGroup_"+ops).data("kendoDropDownList").text(),
            },
            contentType: 'application/json',
            success: function (response) {
                //alert(response);
                $('#txtData').text(response);
                callWIPDataWindow(true);
//                if(response){
//                    alert('ok');
//                }else{
//                    alert("not ok");
//                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }

        });
    }
};

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/11/2016 11:29 AM
 * @description :   log object
 *              :   change this into a modal if available
 */
var _logs = {
    show: function(ops,type,message){
        _logs.clear(ops);
        if(type=='err'){
            $("#lblLogs_"+ops).css('color','#ff0000');
        }else{
            $("#lblLogs_"+ops).css('color','green');
        }

        $("#lblLogs_"+ops).html(message);
    },
    clear: function(ops){
        $("#lblLogs_"+ops).text('');
    },
};

/*
 * @author      :   Dev AC <aabasolo@ALLEGROMICRO.COM>
 * @date        :   7/13/2016 9:00 AM
 * @description :   generic notification modal
 */
function notification_modal(title, message, type) {
    var header_style;
    if (type == "success")
        header_style = 'style="background-color: #1DB198; color: #ffffff;"';
    else if (type == "danger")
        header_style = 'style="background-color: #F25656; color: #ffffff;"';

    var modal = '<div class="modal fade" id="modal_div" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog">';
            modal += '<div class="modal-content">';

                modal += '<div class="modal-header" '+ header_style +'>';
                    modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
                    modal += '<h4 class="modal-title" id="myModalLabel">' + title + '</h4>';
                modal += '</div>';

                modal += '<div class="modal-body">';
                    modal += message;
                modal += '</div>';

                modal += '<div class="modal-footer">';
                    modal += '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>';
                    //modal += '<button type="button" class="btn btn-success">Save changes</button>';
                modal += '</div>';

            modal += '</div>';
        modal += '</div>';
    modal += '</div>';

    $("#notification_modal").html(modal);
    $("#modal_div").modal("show");
    $("#modal_div").css('z-index', '1000000');
}
$("#strLotNoWbSensorBs").change(function () {
    $("#msgSensorBsRow").hide();
});
$("#empNameWbSensorBs").change(function () {
    $("#msgSensorBsRow").hide();
});

$("#strLotNoWbSensorBsPbo").change(function () {
    $("#msgSensorBsPboRow").hide();
});
$("#empNameWbSensorBsPbo").change(function () {
    $("#msgSensorBsPboRow").hide();
});

$("#strLotNoWbSensorWp").change(function () {
    $("#msgSensorWpRow").hide();
});
$("#empNameWbSensorWp").change(function () {
    $("#msgSensorWpRow").hide();
});

function onChangeEquipListWbSensorBs(e) {
    $("#msgSensorBsRow").hide();
}
function onChangeWbWipDataSetupSensorBs(e) {
    $("#msgSensorBsRow").hide();
}
function onChangeEquipListWbSensorBsPbo(e) {
    $("#msgSensorBsPboRow").hide();
}
function onChangeWbWipDataSetupSensorBsPbo(e) {
    $("#msgSensorBsPboRow").hide();
}
function onChangeEquipListWbSensorWp(e) {
    $("#msgSensorWpRow").hide();
}
function onChangeWbWipDataSetupSensorWp(e) {
    $("#msgSensorWpRow").hide();
}

function copyRemarksWbSensorBs(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSensorBs').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbSensorBsPbo(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSensorBsPbo').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbSensorWp(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSensorWp').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function onClickWbSensorBs(e) {
    kendoConsole.log("event :: click (" + $(e.event.target).closest(".k-button").attr("id") + ")");

    var lotNo = $("#strLotNoWbSensorBs").val();

    if ($("#strLotNoWbSensorBs").val().toString() == "") {

        $('#modalEmptyLot').modal('show');

    }

    else {

        $("#btnWbSensorBs").attr("disabled", "disabled");

        $.ajax({
            url: '/WirebondSensor/UpdateTableWbSensor/'
                        , type: 'POST'
                        , data: JSON.stringify({ module: "wirebondSensorBs", lotNumber: lotNo })
                        , contentType: 'application/json'
                        , beforeSend: function () {
                            // Handle the beforeSend event

                            $("#spinner").show();
                        }
                        , complete: function () {
                            // Handle the complete event

                            $("#spinner").hide();
                        }
                        , success: function (lotNumber) {

                            if (lotNumber == "success") {

                                $("#gridWbSensorBs").show();
                                $("#gridWbSensorBs").data("kendoGrid").dataSource.read();
                                $(".k-pager-refresh").trigger('click');
                            }

                            else if (lotNumber == "failed") {

                                $("#gridWbSensorBs").hide();

                                $('#modalNotExist').modal('show');
                            }
                        }

                        , error: function (result) {

                            if (result.status == "500") {


                                alert(result.status + ' ' + result.statusText);
                            }
                            else {

                                alert(result.status + ' ' + result.statusText);
                            }
                        }

        });

        setTimeout('$("#btnWbSensorBs").removeAttr("disabled")', 1500);
    }

    $("#wbWipDataSetupSensorBs").data("kendoDropDownList").dataSource.read();
}

function onClickWbSensorBsPbo(e) {
    kendoConsole.log("event :: click (" + $(e.event.target).closest(".k-button").attr("id") + ")");

    var lotNo = $("#strLotNoWbSensorBsPbo").val();

    if ($("#strLotNoWbSensorBsPbo").val().toString() == "") {

        $('#modalEmptyLot').modal('show');

    }

    else {

        $("#btnWbSensorBsPbo").attr("disabled", "disabled");

        $.ajax({

            url: '/WirebondSensor/UpdateTableWbSensor/'
                        , type: 'POST'
                        , data: JSON.stringify({ module: "wirebondSensorBsPbo", lotNumber: lotNo })
                        , contentType: 'application/json'
                        , beforeSend: function () {
                            // Handle the beforeSend event

                            $("#spinner").show();
                        }
                        , complete: function () {
                            // Handle the complete event

                            $("#spinner").hide();
                        }
                        , success: function (lotNumber) {

                            if (lotNumber == "success") {

                                $("#gridWbSensorBsPbo").show();
                                $("#gridWbSensorBsPbo").data("kendoGrid").dataSource.read();
                                $(".k-pager-refresh").trigger('click');
                            }

                            else if (lotNumber == "failed") {

                                $("#gridWbSensorBsPbo").hide();

                                $('#modalNotExist').modal('show');
                            }
                        }

                        , error: function (result) {

                            if (result.status == "500") {


                                alert(result.status + ' ' + result.statusText);
                            }
                            else {

                                alert(result.status + ' ' + result.statusText);
                            }
                        }

        });

        setTimeout('$("#btnWbSensorBsPbo").removeAttr("disabled")', 1500);
    }

    $("#wbWipDataSetupSensorBsPbo").data("kendoDropDownList").dataSource.read();

}

function onClickWbSensorWp(e) {
    kendoConsole.log("event :: click (" + $(e.event.target).closest(".k-button").attr("id") + ")");

    var lotNo = $("#strLotNoWbSensorWp").val();

    if ($("#strLotNoWbSensorWp").val().toString() == "") {

        $('#modalEmptyLot').modal('show');

    }

    else {

        $("#btnWbSensorWp").attr("disabled", "disabled");

        $.ajax({
            url: '/WirebondSensor/UpdateTableWbSensor/'
                    , type: 'POST'
                    , data: JSON.stringify({ module: "wirebondSensorWp", lotNumber: lotNo })
                    , contentType: 'application/json'
                    , beforeSend: function () {
                        // Handle the beforeSend event
                        $("#spinner").show();
                    }
                    , complete: function () {
                        // Handle the complete event
                        $("#spinner").hide();
                    }
                    , success: function (lotNumber) {

                        if (lotNumber == "success") {

                            $("#gridWbSensorWp").show();
                            $("#gridWbSensorWp").data("kendoGrid").dataSource.read();
                            $(".k-pager-refresh").trigger('click');

                        }

                        else if (lotNumber == "failed") {

                            $("#gridWbSensorWp").hide();

                            $('#modalNotExist').modal('show');

                        }


                    }

                    , error: function (result) {

                        if (result.status == "500") {


                            alert(result.status + ' ' + result.statusText);


                        }
                        else {

                            alert(result.status + ' ' + result.statusText);

                        }


                    }
        });

        setTimeout('$("#btnWbSensorWp").removeAttr("disabled")', 1500);
    }

    $("#wbWipDataSetupSensorWp").data("kendoDropDownList").dataSource.read();
}


$("#customCommandWbSensorBs").click(function (e) {

    $.ajax({

        url: '/WirebondSensor/SPCCalcWirebondSensor_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondSensorBs", strLotNo: $("#strLotNoWbSensorBs").val(), strMachine: $("#equipListWbSensorBs").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbSensorBs").val();

            $('#txtData').text(lotNo);


            $.each(data, function (i, item) {

                var itemName = item.WIPDataValue.toString();
                var remarks = item.WIPDataPrompt.toString();

                if (itemName.match(/\d+/g) && remarks.includes('Remarks')) {
                    itemName;
                }
                else if (itemName.match(/\d+/g) && !remarks.includes('Remarks')) {

                    itemName = parseFloat(itemName).toFixed(2);
                }
                else {
                    itemName;
                }


                $('#txtData').text($('#txtData').val() + "\n" + itemName);

            }); // end of each loop                    


        } // end of success data

    });  // end ajax request

    $("#gridWbSensorBs").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandWbSensorBsPbo").click(function (e) {

    $.ajax({

        url: '/WirebondSensor/SPCCalcWirebondSensor_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondSensorBsPbo", strLotNo: $("#strLotNoWbSensorBsPbo").val(), strMachine: $("#equipListWbSensorBsPbo").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbSensorBsPbo").val();

            $('#txtData').text(lotNo);


            $.each(data, function (i, item) {

                var itemName = item.WIPDataValue.toString();
                var remarks = item.WIPDataPrompt.toString();

                if (itemName.match(/\d+/g) && remarks.includes('Remarks')) {
                    itemName;
                }
                else if (itemName.match(/\d+/g) && !remarks.includes('Remarks')) {

                    itemName = parseFloat(itemName).toFixed(2);
                }
                else {
                    itemName;
                }


                $('#txtData').text($('#txtData').val() + "\n" + itemName);

            }); // end of each loop                    


        } // end of success data

    });  // end ajax request

    $("#gridWbSensorBsPbo").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandWbSensorWp").click(function (e) {

    $.ajax({

        url: '/WirebondSensor/SPCCalcWirebondSensor_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondSensorWp", strLotNo: $("#strLotNoWbSensorWp").val(), strMachine: $("#equipListWbSensorWp").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbSensorWp").val();

            $('#txtData').text(lotNo);


            $.each(data, function (i, item) {

                var itemName = item.WIPDataValue.toString();
                var remarks = item.WIPDataPrompt.toString();

                if (itemName.match(/\d+/g) && remarks.includes('Remarks')) {
                    itemName;
                }
                else if (itemName.match(/\d+/g) && !remarks.includes('Remarks')) {

                    itemName = parseFloat(itemName).toFixed(2);
                }
                else {
                    itemName;
                }

                $('#txtData').text($('#txtData').val() + "\n" + itemName);

            }); // end of each loop                    


        } // end of success data

    });  // end ajax request

    $("#gridWbSensorWp").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandSubmitSensorBs").click(function (e) {

    if ($("#wbWipDataSetupSensorBs").data("kendoDropDownList").text() == "") {

        $("#msgSensorBsRow").show();
        $("#msgSensorBs").val("Unable to submit data, please fill up wip data setup");
        $("#msgSensorBs").css("color", "red");
    }
    else {


        $.ajax({
            url: '/WirebondSensor/SubmitWipDataValuesSensor',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondSensorBs", strLotNo: $("#strLotNoWbSensorBs").val(), strMachine: $("#equipListWbSensorBs").data("kendoDropDownList").text(), strEmployee: $("#empNameWbSensorBs").val(), strWbSensorSetup: $("#wbWipDataSetupSensorBs").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                // Handle the beforeSend event
                $("#spinner").show();
            },
            complete: function () {
                // Handle the complete event
                $("#spinner").hide();
            },
            success: function (data) {

                if (data != "") {

                    $("#msgSensorBsRow").show();
                    $("#msgSensorBs").val(data.msg);
                    $("#msgSensorBs").text(data.msg.substr(0, 5) == "Error" ? $("#msgSensorBs").css("color", "red") : $("#msgSensorBs").css("color", "green"));
                    $("#msgSensorBs").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkSensorBs").val(data.chart);

                    if ($("#chartLinkSensorBs").val() != "") {
                        $("#customCommandloadChartSensorBs").click();
                    }
                }

            }, // end of success data

            error: function (request, status, err) {
                alert(status);
                alert(err);
            }

        });  // end ajax request            

    }

    e.preventDefault();
});


(function ($) {
    // DOM Ready
    $(function () {
        // Binding a click event
        // From jQuery v.1.7.0 use .on() instead of .bind()
        $('#customCommandloadChartSensorBs').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkSensorBs").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);

$("#customCommandSubmitSensorBsPbo").click(function (e) {

    if ($("#wbWipDataSetupSensorBsPbo").data("kendoDropDownList").text() == "") {

        $("#msgSensorBsPboRow").show();
        $("#msgSensorBsPbo").val("Unable to submit data, please fill up wip data setup");
        $("#msgSensorBsPbo").css("color", "red");
    }
    else {

        $.ajax({
            url: '/WirebondSensor/SubmitWipDataValuesSensor',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondSensorBsPbo", strLotNo: $("#strLotNoWbSensorBsPbo").val(), strMachine: $("#equipListWbSensorBsPbo").data("kendoDropDownList").text(), strEmployee: $("#empNameWbSensorBsPbo").val(), strWbSensorSetup: $("#wbWipDataSetupSensorBsPbo").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                // Handle the beforeSend event
                $("#spinner").show();
            },
            complete: function () {
                // Handle the complete event
                $("#spinner").hide();
            },
            success: function (data) {

                if (data != "") {

                    $("#msgSensorBsPboRow").show();
                    $("#msgSensorBsPbo").val(data.msg);
                    $("#msgSensorBsPbo").text(data.msg.substr(0, 5) == "Error" ? $("#msgSensorBsPbo").css("color", "red") : $("#msgSensorBsPbo").css("color", "green"));
                    $("#msgSensorBsPbo").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkSensorBsPbo").val(data.chart);

                    if ($("#chartLinkSensorBsPbo").val() != "") {
                        $("#customCommandloadChartSensorBsPbo").click();
                    }
                }


            }, // end of success data


            error: function (request, status, err) {
                alert(status);
                alert(err);
            }


        });  // end ajax request            
    }

    e.preventDefault();
});

(function ($) {
    // DOM Ready
    $(function () {
        // Binding a click event
        // From jQuery v.1.7.0 use .on() instead of .bind()
        $('#customCommandloadChartSensorBsPbo').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkSensorBsPbo").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);


$("#customCommandSubmitSensorWp").click(function (e) {

    if ($("#wbWipDataSetupSensorWp").data("kendoDropDownList").text() == "") {

        $("#msgSensorWpRow").show();
        $("#msgSensorWp").val("Unable to submit data, please fill up wip data setup");
        $("#msgSensorWp").css("color", "red");

    }
    else {

        $.ajax({
            url: '/WirebondSensor/SubmitWipDataValuesSensor',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondSensorWp", strLotNo: $("#strLotNoWbSensorWp").val(), strMachine: $("#equipListWbSensorWp").data("kendoDropDownList").text(), strEmployee: $("#empNameWbSensorWp").val(), strWbSensorSetup: $("#wbWipDataSetupSensorWp").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                // Handle the beforeSend event
                $("#spinner").show();
            },
            complete: function () {
                // Handle the complete event

                $("#spinner").hide();
            },
            success: function (data) {

                if (data != "") {

                    $("#msgSensorWpRow").show();
                    $("#msgSensorWp").val(data.msg);
                    $("#msgSensorWp").text(data.msg.substr(0, 5) == "Error" ? $("#msgSensorWp").css("color", "red") : $("#msgSensorWp").css("color", "green"));
                    $("#msgSensorWp").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkSensorWp").val(data.chart);

                    if ($("#chartLinkSensorWp").val() != "") {
                        $("#customCommandloadChartSensorWp").click();
                    }
                }


            }, // end of success data


            error: function (request, status, err) {
                alert(status);
                alert(err);
            }

        });  // end ajax request                                   


    }
    e.preventDefault();
});


(function ($) {
    // DOM Ready
    $(function () {
        // Binding a click event
        // From jQuery v.1.7.0 use .on() instead of .bind()
        $('#customCommandloadChartSensorWp').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkSensorWp").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);
$("#strLotNoSohedBs").change(function () {
    $("#msgSohedBsRow").hide();
});
$("#empNameSohedBs").change(function () {
    $("#msgSohedBsRow").hide();
});


$("#strLotNoSohedBsPbo").change(function () {
    $("#msgSohedBsPboRow").hide();
});
$("#empNameSohedBsPbo").change(function () {
    $("#msgSohedBsPboRow").hide();
});


$("#strLotNoSohedWp").change(function () {
    $("#msgSohedWpRow").hide();
});
$("#empNameSohedWp").change(function () {
    $("#msgSohedWpRow").hide();
});

function onChangeEquipListSohedBs(e) {
    $("#msgSohedBsRow").hide();
}

function onChangeWbWipDataSetupSohedBs(e) {
    $("#msgSohedBsRow").hide();
}

function onChangeEquipListSohedBsPbo(e) {
    $("#msgSohedBsPboRow").hide();
}

function onChangeWbWipDataSetupSohedBsPbo(e) {
    $("#msgSohedBsPboRow").hide();
}

function onChangeEquipListSohedWp(e) {
    $("#msgSohedWpRow").hide();
}

function onChangeWbWipDataSetupSohedWp(e) {
    $("#msgSohedWpRow").hide();
}

function copyRemarksWbSohedBs(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSohedBs').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbSohedBsPbo(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSohedBsPbo').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbSohedWp(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSohedWp').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbSohedWpPbo(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSohedWpPbo').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

$("#customCommandSohedBs").click(function (e) {

    $.ajax({

        url: '/WirebondSohed/SPCCalcWirebondSohed_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondSohedBs", strLotNo: $("#strLotNoSohedBs").val(), strMachine: $("#equipListSohedBs").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoSohedBs").val();

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

    $("#gridWbSohedBs").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});


$("#customCommandSohedBsPbo").click(function (e) {

    $.ajax({

        url: '/WirebondSohed/SPCCalcWirebondSohed_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondSohedBsPbo", strLotNo: $("#strLotNoSohedBsPbo").val(), strMachine: $("#equipListSohedBsPbo").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoSohedBsPbo").val();

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

    $("#gridWbSohedBsPbo").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});


$("#customCommandSohedWp").click(function (e) {

    $.ajax({

        url: '/WirebondSohed/SPCCalcWirebondSohed_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondSohedWp", strLotNo: $("#strLotNoSohedWp").val(), strMachine: $("#equipListSohedWp").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoSohedWp").val();

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

    $("#gridWbSohedWp").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandSohedWpPbo").click(function (e) {

    $.ajax({
        url: '/Home/WIPDataPrompt_WbWPTPBOSohed',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ strLotNoSohedWpPbo: $("#strLotNoSohedWpPbo").val() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoSohedWpPbo").val();

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

    $("#gridWbSohedWpPbo").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandSubmitSohedBs").click(function (e) {

    if ($("#wbWipDataSetupSohedBs").data("kendoDropDownList").text() == "") {

        $("#msgSohedBsRow").show();
        $("#msgSohedBs").val("Unable to submit data, please fill up wip data setup");
        $("#msgSohedBs").css("color", "red");
    }
    else {

        $.ajax({
            url: '/WirebondSohed/SubmitWipDataValuesSohed',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondSohedBs", strLotNo: $("#strLotNoSohedBs").val(), strMachine: $("#equipListSohedBs").data("kendoDropDownList").text(), strEmployee: $("#empNameSohedBs").val(), strWbSohedSetup: $("#wbWipDataSetupSohedBs").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
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

                    $("#msgSohedBsRow").show();
                    $("#msgSohedBs").val(data.msg);
                    $("#msgSohedBs").text(data.msg.substr(0, 5) == "Error" ? $("#msgSohedBs").css("color", "red") : $("#msgSohedBs").css("color", "green"));
                    $("#msgSohedBs").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkSohedBs").val(data.chart);

                    if ($("#chartLinkSohedBs").val() != "") {
                        $("#customCommandloadChartSohedBs").click();
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
        $('#customCommandloadChartSohedBs').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkSohedBs").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);

$("#customCommandSubmitSohedBsPbo").click(function (e) {

    if ($("#wbWipDataSetupSohedBsPbo").data("kendoDropDownList").text() == "") {

        $("#msgSohedBsPboRow").show();
        $("#msgSohedBsPbo").val("Unable to submit data, please fill up wip data setup");
        $("#msgSohedBsPbo").css("color", "red");
    }
    else {

        $.ajax({
            url: '/WirebondSohed/SubmitWipDataValuesSohed',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondSohedBsPbo", strLotNo: $("#strLotNoSohedBsPbo").val(), strMachine: $("#equipListSohedBsPbo").data("kendoDropDownList").text(), strEmployee: $("#empNameSohedBsPbo").val(), strWbSohedSetup: $("#wbWipDataSetupSohedBsPbo").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                // Handle the beforeSend event

                $("#spinner").show();
            }
                                        , complete: function () {
                                            // Handle the complete event

                                            $("#spinner").hide();
                                        },
            success: function (data) {

                if (data != "") {

                    $("#msgSohedBsPboRow").show();
                    $("#msgSohedBsPbo").val(data.msg);
                    $("#msgSohedBsPbo").text(data.msg.substr(0, 5) == "Error" ? $("#msgSohedBsPbo").css("color", "red") : $("#msgSohedBsPbo").css("color", "green"));
                    $("#msgSohedBsPbo").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkSohedBsPbo").val(data.chart);

                    if ($("#chartLinkSohedBsPbo").val() != "") {
                        $("#customCommandloadChartSohedBsPbo").click();
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
        $('#customCommandloadChartSohedBsPbo').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkSohedBsPbo").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);

$("#customCommandSubmitSohedWp").click(function (e) {

    if ($("#wbWipDataSetupSohedWp").data("kendoDropDownList").text() == "") {

        $("#msgSohedWpRow").show();
        $("#msgSohedWp").val("Unable to submit data, please fill up wip data setup");
        $("#msgSohedWp").css("color", "red");
    }
    else {

        $.ajax({
            url: '/WirebondSohed/SubmitWipDataValuesSohed',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondSohedWp", strLotNo: $("#strLotNoSohedWp").val(), strMachine: $("#equipListSohedWp").data("kendoDropDownList").text(), strEmployee: $("#empNameSohedWp").val(), strWbSohedSetup: $("#wbWipDataSetupSohedWp").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
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

                    $("#msgSohedWpRow").show();
                    $("#msgSohedWp").val(data.msg);
                    $("#msgSohedWp").text(data.msg.substr(0, 5) == "Error" ? $("#msgSohedWp").css("color", "red") : $("#msgSohedWp").css("color", "green"));
                    $("#msgSohedWp").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkSohedWp").val(data.chart);

                    if ($("#chartLinkSohedWp").val() != "") {
                        $("#customCommandloadChartSohedWp").click();
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
        $('#customCommandloadChartSohedWp').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkSohedWp").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);
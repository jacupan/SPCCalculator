$("#strLotNoWbGtsBs").change(function () {
    $("#msgGtsBsRow").hide();
});
$("#empNameWbGtsBs").change(function () {
    $("#msgGtsBsRow").hide();
});


$("#strLotNoWbGtsBsPbo").change(function () {
    $("#msgGtsBsPboRow").hide();
});
$("#empNameWbGtsBsPbo").change(function () {
    $("#msgGtsBsPboRow").hide();
});


$("#strLotNoWbGtsWp").change(function () {
    $("#msgGtsWpRow").hide();
});
$("#empNameWbGtsWp").change(function () {
    $("#msgGtsWpRow").hide();
});

function onChangeEquipListWbGtsBs(e) {
    $("#msgGtsBsRow").hide();
}
function onChangeWbWipDataSetupGtsBs(e) {
    $("#msgGtsBsRow").hide();
}
function onChangeEquipListWbGtsBsPbo(e) {
    $("#msgGtsBsPboRow").hide();
}

function onChangeWbWipDataSetupGtsBsPbo(e) {
    $("#msgGtsBsPboRow").hide();
}

function onChangeEquipListWbGtsWp(e) {
    $("#msgGtsWpRow").hide();
}

function onChangeWbWipDataSetupGtsWp(e) {
    $("#msgGtsWpRow").hide();
}

function copyRemarksWbGtsBs(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbGtsBs').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbGtsBsPbo(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbGtsBsPbo').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbGtsWp(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbGtsWp').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbGtsWpPbo(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbGtsWpPbo').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}


$("#customCommandWbGtsBs").click(function (e) {

    $.ajax({

        url: '/WirebondGts/SPCCalcWirebondGts_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondGtsBs", strLotNo: $("#strLotNoWbGtsBs").val(), strMachine: $("#equipListWbGtsBs").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbGtsBs").val();

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

    $("#gridWbGtsBs").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandWbGtsBsPbo").click(function (e) {

    $.ajax({

        url: '/WirebondGts/SPCCalcWirebondGts_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondGtsBsPbo", strLotNo: $("#strLotNoWbGtsBsPbo").val(), strMachine: $("#equipListWbGtsBsPbo").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbGtsBsPbo").val();

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

    $("#gridWbGtsBsPbo").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandWbGtsWp").click(function (e) {

    $.ajax({

        url: '/WirebondGts/SPCCalcWirebondGts_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondGtsWp", strLotNo: $("#strLotNoWbGtsWp").val(), strMachine: $("#equipListWbGtsWp").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbGtsWp").val();

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

    $("#gridWbGtsWp").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandWbGtsWpPbo").click(function (e) {

    $.ajax({
        url: '/Home/WIPDataPrompt_WbGtsWpPbo',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ strLotNoWbGtsWpPbo: $("#strLotNoWbGtsWpPbo").val() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbGtsWpPbo").val();

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

    $("#gridWbGtsWpPbo").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});

$("#customCommandSubmitGtsBs").click(function (e) {

    if ($("#wbWipDataSetupGtsBs").data("kendoDropDownList").text() == "") {

        $("#msgGtsBsRow").show();
        $("#msgGtsBs").val("Unable to submit data, please fill up wip data setup");
        $("#msgGtsBs").css("color", "red");
    }
    else {


        $.ajax({
            url: '/WirebondGts/SubmitWipDataValuesGts',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondGtsBs", strLotNo: $("#strLotNoWbGtsBs").val(), strMachine: $("#equipListWbGtsBs").data("kendoDropDownList").text(), strEmployee: $("#empNameWbGtsBs").val(), strWbGtsSetup: $("#wbWipDataSetupGtsBs").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
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

                    $("#msgGtsBsRow").show();
                    $("#msgGtsBs").val(data.msg);
                    $("#msgGtsBs").text(data.msg.substr(0, 5) == "Error" ? $("#msgGtsBs").css("color", "red") : $("#msgGtsBs").css("color", "green"));
                    $("#msgGtsBs").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkGtsBs").val(data.chart);

                    if ($("#chartLinkGtsBs").val() != "") {
                        $("#customCommandloadChartGtsBs").click();
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
        $('#customCommandloadChartGtsBs').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkGtsBs").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);

$("#customCommandSubmitGtsBsPbo").click(function (e) {

    if ($("#wbWipDataSetupGtsBsPbo").data("kendoDropDownList").text() == "") {

        $("#msgGtsBsPboRow").show();
        $("#msgGtsBsPbo").val("Unable to submit data, please fill up wip data setup");
        $("#msgGtsBsPbo").css("color", "red");
    }
    else {


        $.ajax({
            url: '/WirebondGts/SubmitWipDataValuesGts',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondGtsBsPbo", strLotNo: $("#strLotNoWbGtsBsPbo").val(), strMachine: $("#equipListWbGtsBsPbo").data("kendoDropDownList").text(), strEmployee: $("#empNameWbGtsBsPbo").val(), strWbGtsSetup: $("#wbWipDataSetupGtsBsPbo").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
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

                    $("#msgGtsBsPboRow").show();
                    $("#msgGtsBsPbo").val(data.msg);
                    $("#msgGtsBsPbo").text(data.msg.substr(0, 5) == "Error" ? $("#msgGtsBsPbo").css("color", "red") : $("#msgGtsBsPbo").css("color", "green"));
                    $("#msgGtsBsPbo").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkGtsBsPbo").val(data.chart);

                    if ($("#chartLinkGtsBsPbo").val() != "") {
                        $("#customCommandloadChartGtsBsPbo").click();
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
        $('#customCommandloadChartGtsBsPbo').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkGtsBsPbo").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);


$("#customCommandSubmitGtsWp").click(function (e) {

    if ($("#wbWipDataSetupGtsWp").data("kendoDropDownList").text() == "") {

        $("#msgGtsWpRow").show();
        $("#msgGtsWp").val("Unable to submit data, please fill up wip data setup");
        $("#msgGtsWp").css("color", "red");

    }
    else {

        $.ajax({
            url: '/WirebondGts/SubmitWipDataValuesGts',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondGtsWp", strLotNo: $("#strLotNoWbGtsWp").val(), strMachine: $("#equipListWbGtsWp").data("kendoDropDownList").text(), strEmployee: $("#empNameWbGtsWp").val(), strWbGtsSetup: $("#wbWipDataSetupGtsWp").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
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

                    $("#msgGtsWpRow").show();
                    $("#msgGtsWp").val(data.msg);
                    $("#msgGtsWp").text(data.msg.substr(0, 5) == "Error" ? $("#msgGtsWp").css("color", "red") : $("#msgGtsWp").css("color", "green"));
                    $("#msgGtsWp").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkGtsWp").val(data.chart);

                    if ($("#chartLinkGtsWp").val() != "") {
                        $("#customCommandloadChartGtsWp").click();
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
        $('#customCommandloadChartGtsWp').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkGtsWp").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);
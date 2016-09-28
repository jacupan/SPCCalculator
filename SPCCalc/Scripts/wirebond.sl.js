$("#strLotNoWbSLBs").change(function () {
    $("#msgSLBsRow").hide();
});
$("#empNameWbSLBs").change(function () {
    $("#msgSLBsRow").hide();
});

$("#strLotNoWbSLWp").change(function () {
    $("#msgSLWpRow").hide();
});
$("#empNameWbSLWp").change(function () {
    $("#msgSLWpRow").hide();
});

function onChangeEquipListWbSLBs(e) {
    $("#msgSLBsRow").hide();
}
function onChangeWbWipDataSetupSLBs(e) {
    $("#msgSLBsRow").hide();
}

function onChangeEquipListWbSLWp(e) {
    $("#msgSLWpRow").hide();
}

function onChangeWbWipDataSetupSLWp(e) {
    $("#msgSLWpRow").hide();
}

function copyRemarksWbSLBs(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSLBs').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}

function copyRemarksWbSLWp(data) {

    var items = this.dataSource.data();
    for (var i = 0; i < items.length; i++) {
        var remarks = $("#Remarks").val();
        items[i].set("Remarks", remarks);
    }
    $('#gridWbSLWp').data('kendoGrid').dataSource.sync();
    this.refresh();

    $('#modalUpdateSuccess').modal('show');

}


$("#customCommandSLBs").click(function (e) {

    $.ajax({

        url: '/WirebondSL/SPCCalcWirebondSL_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondSLBs", strLotNo: $("#strLotNoWbSLBs").val(), strMachine: $("#equipListWbSLBs").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbSLBs").val();

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

    $("#gridWbSLBs").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});



$("#customCommandSLWp").click(function (e) {

    $.ajax({

        url: '/WirebondSL/SPCCalcWirebondSL_CamstarReading',
        type: 'POST',
        datatype: 'json',
        data: JSON.stringify({ module: "wirebondSLWp", strLotNo: $("#strLotNoWbSLWp").val(), strMachine: $("#equipListWbSLWp").data("kendoDropDownList").text() }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            var lotNo = $("#strLotNoWbSLWp").val();

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

    $("#gridWbSLWp").data('kendoGrid').dataSource.data([]);

    callWIPDataWindow(true);

    e.preventDefault();
});



$("#customCommandSubmitSLBs").click(function (e) {

    if ($("#wbWipDataSetupSLBs").data("kendoDropDownList").text() == "") {

        $("#msgSLBsRow").show();
        $("#msgSLBs").val("Unable to submit data, please fill up wip data setup");
        $("#msgSLBs").css("color", "red");
    }
    else {


        $.ajax({
            url: '/WirebondSL/SubmitWipDataValuesSL',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondSLBs", strLotNo: $("#strLotNoWbSLBs").val(), strMachine: $("#equipListWbSLBs").data("kendoDropDownList").text(), strEmployee: $("#empNameWbSLBs").val(), strWbSLSetup: $("#wbWipDataSetupSLBs").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
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

                    $("#msgSLBsRow").show();
                    $("#msgSLBs").val(data.msg);
                    $("#msgSLBs").text(data.msg.substr(0, 5) == "Error" ? $("#msgSLBs").css("color", "red") : $("#msgSLBs").css("color", "green"));
                    $("#msgSLBs").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkSLBs").val(data.chart);

                    if ($("#chartLinkSLBs").val() != "") {
                        $("#customCommandloadChartSLBs").click();
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
        $('#customCommandloadChartSLBs').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkSLBs").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);


$("#customCommandSubmitSLWp").click(function (e) {

    if ($("#wbWipDataSetupSLWp").data("kendoDropDownList").text() == "") {

        $("#msgSLWpRow").show();
        $("#msgSLWp").val("Unable to submit data, please fill up wip data setup");
        $("#msgSLWp").css("color", "red");

    }
    else {

        $.ajax({
            url: '/WirebondSL/SubmitWipDataValuesSL',
            type: 'POST',
            datatype: 'json',
            data: JSON.stringify({ module: "wirebondSLWp", strLotNo: $("#strLotNoWbSLWp").val(), strMachine: $("#equipListWbSLWp").data("kendoDropDownList").text(), strEmployee: $("#empNameWbSLWp").val(), strWbSLSetup: $("#wbWipDataSetupSLWp").data("kendoDropDownList").text(), strCompName: $("#compName").val() }),
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

                    $("#msgSLWpRow").show();
                    $("#msgSLWp").val(data.msg);
                    $("#msgSLWp").text(data.msg.substr(0, 5) == "Error" ? $("#msgSLWp").css("color", "red") : $("#msgSLWp").css("color", "green"));
                    $("#msgSLWp").autogrow({ vertical: true, horizontal: false });

                    $("#chartLinkSLWp").val(data.chart);

                    if ($("#chartLinkSLWp").val() != "") {
                        $("#customCommandloadChartSLWp").click();
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
        $('#customCommandloadChartSLWp').bind('click', function (e) {
            e.preventDefault();
            $('#element_to_pop_up').bPopup({
                speed: 450,
                transition: 'slideDown',
                loadUrl: $("#chartLinkSLWp").val(),
                content: 'iframe',
                iframeAttr: 'width="1020" height="820"'
            });
        });
    });
})(jQuery);
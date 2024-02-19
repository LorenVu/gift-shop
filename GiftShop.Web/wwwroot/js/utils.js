window.utils = {
    blockUI: function () {
        $.blockUI({
            message:
                '<div class="spinner-border text-primary" role="status"></div>',
            css: {
                backgroundColor: "transparent",
                border: "0",
            },
            overlayCSS: {
                backgroundColor: "#fff",
                opacity: 0.8,
            },
        });
    },

    unblockUI: function () {
        $.unblockUI();
    },
    convertToInt: function (input) {
        let output = input.split(",").join("");
        return output;
    },
    closePopup: function () {
        $(".modal-backdrop").remove();
        $("#myModal").modal("hide");
       
        $(".divPopup").html('');
        var $body = $(document.body);
        $body.css("overflow", "auto");
        $body.width("auto");

        document.body.classList.remove('modal-open');
        document.body.removeAttribute('style');
    },
    closePopupDetail: function () {
        $(".modal-backdrop").remove();
        $("#myModalDetail").modal("hide");
        $(".divPopupDetail").html('');

        var $body = $(document.body);
        $body.css("overflow", "auto");
        $body.width("auto");

        document.body.classList.remove('modal-open');
        document.body.removeAttribute('style');
    },

    showPopup: function (data) {
        
        $(".modal-backdrop").remove();
        var html = "";
        html += "<div class=\"modal fade\" id=\"myModal\" tabindex='-1' role=\"dialog\">";
        html += "<div class=\"modal-dialog modal-dialog-centered modal-lg\" role=\"document\">";
        html += "<div class=\"modal-content\">";

        html += data;

        html += "</div>";
        html += " </div>";
        html += "</div>";

        $(".divPopup").html(html);
        $("#myModal").modal("show");
    },
    showPopupDetail: function (data) {
        
        //$(".modal-backdrop").remove();
        var html = "";
        html += "<div class=\"modal fade\" id=\"myModalDetail\" tabindex='-1' role=\"dialog\">";
        html += "<div class=\"modal-dialog modal-dialog-centered modal-lg\" role=\"document\">";
        html += "<div class=\"modal-content\">";

        html += data;

        html += "</div>";
        html += " </div>";
        html += "</div>";

        $(".divPopupDetail").html(html);
        $("#myModalDetail").modal("show");
    },

    showPopupXL: function (data) {
        $(".modal-backdrop").remove();
        var html = "";
        html += "<div class=\"modal fade\" id=\"myModal\" tabindex='-1' role=\"dialog\">";
        html += "<div class=\"modal-dialog modal-dialog-centered modal-xl\" role=\"document\">";
        html += "<div class=\"modal-content\">";

        html += data;

        html += "</div>";
        html += " </div>";
        html += "</div>";

        $(".divPopup").html(html);
        $("#myModal").modal("show");
    },

    showPopupSM: function (data) {

        $(".modal-backdrop").remove();
        var html = "";
        html += "<div class=\"modal fade\" id=\"myModal\" tabindex='-1' role=\"dialog\">";
        html += "<div class=\"modal-dialog modal-dialog-centered modal\" role=\"document\">";
        html += "<div class=\"modal-content\">";

        html += data;

        html += "</div>";
        html += " </div>";
        html += "</div>";

        $(".divPopup").html(html);
        $("#myModal").modal("show");
    },
    getCandidate: function (JobID, RoundID) {
        $(".modal-backdrop").remove();
        var html = "";
        html += "<div class=\"modal fade\" id=\"myModal\" tabindex='-1' role=\"dialog\" aria-hidden=\"true\" data-keyboard=\"false\" data-backdrop=\"static\">";
        html += "<div class=\"modal-dialog modal-dialog-centered modal-xl\">";
        html += "<div class=\"modal-content\">";

        html += '<div class="modal-header">  ';
        html += '    <a class="close btn" href="">';
        html += '        <span aria-hidden="true">&times;</span>';
        html += '    </a>';
        html += '</div>';

        html += '<div class="modal-body">';        
        html += "<iframe src='/RecruitmentProcess/CandidatePopup?JobID=" + JobID + "&RoundID=" + RoundID + "'' width=\"100%\" height=\"800\" style='border:none'></iframe>";
        html += '</div>';

        html += "</div>";
        html += " </div>";
        html += "</div>";

        $(".divPopup").html(html);
        $("#myModal").modal("show");
    },

    chart: function (type,data) {
        var chartWrapper = $('.chartjs');
        if (chartWrapper.length) {                                    
            try {
                var data_ = JSON.parse(data);

                if (data_ != null && data_.length > 0) {
                    // Bổ sung canvas                    
                    var canvas = "";
                    for (var i = 0; i < data_.length - 1; i++) {
                        canvas += '<div id="container' + (i + 1) + '" class="mb-2" style="height:400px;"></div>';
                    }
                    $('.chartjs').after(canvas);

                    // Vẽ biểu đồ
                    for (var i = 0; i < data_.length; i++) {
                        var obj = data_[i];
                        utils.chartBase('container' + i, type, JSON.stringify(obj));
                    }
                }
            } catch (e) {                
                console.log(e);
            }            
        }        
    },

    chartBase: function (chartWrapper, type, data) { 
        $("#" + chartWrapper).wrap($('<div class="col-md-6"></div>'));
        var tmp = JSON.parse(data);
        if (chartWrapper != null) {

            if (type == 'column' || type == 'line') {
                var barChartExample = Highcharts.chart(chartWrapper, {
                    chart: {
                        type: type
                    },
                    title: {
                        text: tmp[0].title
                    },
                    yAxis: {
                        title: {
                            text: ''
                        }
                    },
                    xAxis: {
                        categories: tmp[0].categories,
                        gridLineWidth: 1
                    },
                    legend: {
                        align: 'right',
                        verticalAlign: 'top',
                        borderWidth: 0
                    },
                    plotOptions: {
                        column: {
                            stacking: 'normal',
                            dataLabels: {
                                enabled: true
                            }
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    colors: ["Salmon", "LightSeaGreen", "Orange", "YellowGreen", "PeachPuff", "PaleGreen", "PeachPuff"],                    
                    responsive: {
                        rules: [{
                            condition: {
                                maxWidth: 500
                            },
                            chartOptions: {
                                legend: {
                                    layout: 'horizontal',
                                    align: 'center',
                                    verticalAlign: 'bottom'
                                }
                            }
                        }]
                    },
                    series: tmp
                });
            }
        }
    },

    showPopupConfirm: function (msg) {
        var html = "";
        html+='<div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">'
        html+='    <div class="modal-dialog modal-dialog-centered" role="document">                                                                     '
        html+='        <div class="modal-content">                                                                                                      '
        html+='            <div class="modal-header">                                                                                                   '
        html+='                <h5 class="modal-title" id="exampleModalCenterTitle">Xác nhận</h5>                                            '
        html+='                <button type="button" class="close" data-dismiss="modal" aria-label="Close">                                             '
        html+='                    <span aria-hidden="true">&times;</span>                                                                              '
        html+='                </button>                                                                                                                '
        html+='            </div>                                                                                                                       '
        html+='            <div class="modal-body">                                                                                                     '
        html += '                <p>                                                                                                                      '
        html += msg;
        html+='                                                    </p>                                                                                 '
        html+='            </div>                                                                                                                       '
        html+='            <div class="modal-footer">                                                                                                   '
        html +='                <a class="btn btn-primary btnConfirm" data-dismiss="modal">Đồng ý</a>                                       '
        html+='            </div>                                                                                                                       '
        html+='        </div>                                                                                                                           '
        html+='    </div>                                                                                                                               '
        html += '</div>                                                                                                                                   ';

        $(".divPopup").html(html);
        $("#exampleModalCenter").modal("show");
    },
    setShortenText: function () {
        $('.short-view').collapser({
            mode: 'lines',
            truncate: 2, // Shows only 20 words
            showText: 'xem thêm',
            hideText: ' thu gọn',
            ellipsis: ' ... ',
            speed: 'fast',
        });

        $('.short-view-v2').collapser({
            mode: 'chars',
            truncate: 64, // Shows only 10 words
            showText: '[xem thêm]',
            hideText: ' [thu gọn]',
            ellipsis: ' ... ',
            speed: 'fast',
        });

        $('.short-view-v3').collapser({
            mode: 'chars',
            truncate: 128, // Shows only 10 words
            showText: '[xem thêm]',
            hideText: ' [thu gọn]',
            ellipsis: ' ... ',
            speed: 'fast',
        });
        $('.short-view-v4').collapser({
            mode: 'lines',
            truncate: 2, // Shows only 20 words
            showText: 'Hiển thị',
            hideText: 'Thu gọn',
            ellipsis: ' ... ',
            speed: 'fast',
        });
    }
}
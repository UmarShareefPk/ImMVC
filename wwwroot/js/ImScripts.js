$(document).ready(function () {
    $('.dropdown-toggle').dropdown();   
    $("nav").find("li").on("click", "a", function () {
        $('.navbar-collapse.in').collapse('hide');
    });

    $(".notification-icon").click(function () {
        console.log("clicked");
    });
});

window.auto_grow = (event) => {
    // for textareas
    let element = event.target;
    element.style.height = '5px';
    element.style.height = element.scrollHeight + 'px';
}

window.clickTest = (e) => {
    console.log(e);
    event.stopPropagation(); 
    return "yes";
   // DotNet.invokeMethodAsync('ImBlazorWebAssembly', 'callMeFromJS');
}

window.fileuploadChanged = (source, infoId) => {   
    let count = $(source).prop('files').length;
    let info = count > 1 ? count + " files selected" : count + " file selected";
    $("#" + infoId).html(info);
}

window.getMoment = (date) => {
    console.log("hi from script file");
    return moment(date).fromNow();   
};

window.drawOverallChart = (data) => {
      Highcharts.chart('overallChart', {
        chart: {
            type: 'pie',           
        },
        title: {
            text: ''
        },
        credits:
        {
            enabled: false
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y} ({point.percentage:.1f}%)</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
          },
       series: [{
            name: 'Share',
            data: [                
                {
                    name: 'New', y: data.new, color: {
                        radialGradient: [0, 0, 0, 300],
                        stops: [
                            [0, 'rgba(222, 184, 68, 1)'],
                            [1, 'rgba(250, 196, 19, 1)']
                        ]
                    }
                },          
                {
                    name: 'In Progress', y: data.inProgress, color: {
                        radialGradient: [0, 0, 0, 300],
                        stops: [
                            [0, 'rgba(113, 166, 247, 1)'],
                            [1, 'rgba(48, 128, 204, 1)']
                        ]
                    }
                },
                {
                    name: 'Closed', y: data.closed, color: {
                        radialGradient: [0, 0, 0, 300],
                        stops: [
                            [0, 'rgba(66, 194, 56, 1)'],
                            [1, 'rgba(113, 200, 120, 1)']
                        ]
                    }
                },
          
                {
                    name: 'Approved', y: data.approved, color: {
                        radialGradient: [0, 0, 0, 300],
                        stops: [
                            [0, 'rgba(12, 99, 5, 1)'],
                            [1, 'rgba(14, 131, 22, 1)']
                        ]
                    }
                },
                {
                    name: 'Late', y: data.late, color: {
                        radialGradient: [0, 0, 0, 300],
                        stops: [
                            [0, 'rgba(218, 30, 28, 1)'],
                            [1, 'rgba(131, 55, 14, 1)']
                        ]
                    }
                }
                
            ]
        }]
    });

    return "okay";
}

window.drawMostAssignedToUserChart = (data) => {
  //  console.log(data); rgba(32,96,26,1) 0%, 

    let bgColor = {
        linearGradient: [0, 0, 0, 300],
        stops: [
            [0, 'rgba(6,131,181,1)'], 
            [1, 'rgba(7,65,105,1)']                                 
        ]
    };

    data = [       
        {
            name: data[0].Name, y: parseInt(data[0].Count), color: bgColor
        },
        {
            name: data[1].Name, y: parseInt(data[1].Count), color: bgColor
        },
        {
            name: data[2].Name, y: parseInt(data[2].Count), color: bgColor
        },
        {
            name: data[3].Name, y: parseInt(data[3].Count), color: bgColor
        },
        {
            name: data[4].Name, y: parseInt(data[4].Count), color: bgColor
        }
    ];

    Highcharts.chart('mostAssignedtoUsersChart', {
        title: {
            text: 'My chart'
        },
        chart: {
            type: 'bar',            
            //width: (100) + '%',
        },
        title: {
            text: ''
        },
        credits:
        {
            enabled: false
        },
        legend: {
            enabled: false
        },
        yAxis: {
            title: {
                text: ''
            }
        },

        xAxis: {
            type: 'category',
            min: 0,
            labels: {
                animate: true
            }
        },

        series: [{
            name: '',
            dataSorting: {
                enabled: true,
                sortKey: 'y'
            },
            data: data
        }]
    });

    return "okay";
}

window.downloadIncidentFile = (baseUrl, file) => {
    //console.log(file);
    window.open(
        baseUrl + "/Incidents/DownloadFile?"
        + "type=incident"
        + "&commentId=" + ""
        + "&incidentId=" + file.incidentId
        + "&filename=" + file.fileName
        + "&contentType=" + file.contentType
    );
    return "Ok";
}

window.downloadCommentFile = (baseUrl, incidentId, file) => {
    window.open(
        baseUrl + "/Incidents/DownloadFile?" +
        "type=comment" +
        "&commentId=" + file.commentId +
        "&incidentId=" + incidentId +
        "&filename=" + file.fileName +
        "&contentType=" + file.contentType
    );
    return "Ok";
}

window.addIncident = async (baseUrl, fields) => {  
 

    let files = $("#" + fields.files).prop('files');
    console.log(fields);
    console.log(files);
    
    const formData = new FormData();

    if (files) {
        for (let i = 0; i < files.length; i++) {
            formData.append(
                "Attachment" + i + 1,
                files[i],
                files[i].name
            );
        }
    }
    formData.append("CreatedBy", fields.userId);
    formData.append("AssignedTo", fields.assignee);
    formData.append("Title", fields.title);
    formData.append("Description", fields.description);
    formData.append("AdditionalDeta", fields.additionalDetails);
    formData.append("StartTime", fields.startTime);
    formData.append("DueDate", fields.dueDate);
    formData.append("Status", "N"); 

    let url = baseUrl + "/Incidents/AddIncident";

    let res = await fetch(url, {
        method: 'post',
        headers: new Headers({ 'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem("token")) }),
        body: formData,
    })
        .then(res => {
            $("#" + fields.files).val(null);
            $("#" + fields.files).replaceWith($("#" + fields.files).clone(true));
            $("#incidentFileuploadInfo").html("Click here to upload files");
            $('#exampleModalCenter').modal('hide');
            return true;
        }).catch(err => console.log(err));
    return true;
}

window.addComment = async (baseUrl,commentText, commentFilesId, incidentId, userId) => {
  
    let files = $("#" + commentFilesId).prop('files'); 

    if (commentText.trim() === "") {
        alert("Please add comment first.");
        return;
    }
    const formData = new FormData();

    if (files) {
        for (let i = 0; i < files.length; i++) {
            formData.append(
                "Attachment" + i + 1,
                files[i],
                files[i].name
            );
        }
    }
    formData.append("CommentText", commentText.trim());
    formData.append("IncidentId", incidentId);
    formData.append("UserId", userId);
    
    let url = baseUrl + "/Incidents/AddComment";

   let res = await fetch(url, {
        method: 'post',
        headers: new Headers({ 'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem("token")) }),
        body: formData,
    })
       .then(res => res.json()).then(res => {     
           $("#" + commentFilesId).val(null);
            $("#" + commentFilesId).replaceWith($("#" + commentFilesId).clone(true));
            $("#commentFileuploadInfo").html("Click here to upload files");
        return res;
    }).catch(err => console.log(err));

    return res;
}
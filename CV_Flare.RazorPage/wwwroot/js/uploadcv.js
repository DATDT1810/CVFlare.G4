$(document).ready(function () {
    // Chọn file khi nhấn nút "Chọn CV"
    $("#chooseFileButton").click(function () {
        $("#cvFileInput").click();  // Mở hộp thoại chọn file CV
    });

    // Tải lên mô tả công việc
    $("#uploadJobDescBtn").click(function () {
        var jobDescriptionText = $("#jobDescriptionText").val();  // Lấy mô tả công việc
        var jobDescFile = $("#jobDescFile")[0].files[0];  // Lấy file mô tả nếu có

        var formData = new FormData();
        formData.append('JobDescriptionText', jobDescriptionText);

        if (jobDescFile) {
            formData.append('JobDescriptionFile', jobDescFile);
        }

        // Gửi yêu cầu upload mô tả công việc
        $.ajax({
            url: '/api/CvSubmission/uploadJobDescription',  // Đảm bảo API URL đúng
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                alert('Mô tả công việc đã được tải lên thành công!');
            },
            error: function (error) {
                alert('Có lỗi khi tải lên mô tả!');
            }
        });
    });

    // Tải lên CV
    $("#uploadCVBtn").click(function () {
        var cvFile = $("#cvFileInput")[0].files[0];  // Lấy file CV

        if (!cvFile || cvFile.type !== 'application/pdf') {
            alert('Vui lòng tải lên file CV dưới dạng PDF.');
            return;
        }

        var formData = new FormData();
        formData.append('CVFile', cvFile);

        // Gửi yêu cầu upload CV
        $.ajax({
            url: '/api/CvSubmission/uploadCV',  // Đảm bảo API URL đúng
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                alert('CV đã được tải lên thành công!');
            },
            error: function (error) {
                alert('Có lỗi khi tải lên CV!');
            }
        });
    });
});

function getAllCommentsInPost(postId) {
    $.ajax({
        type: "GET",
        url: '/User/Comment/GetAllCommentsInPost/',
        data: {
            postId: postId,
        },
        dataType: 'json',
        success: function (data) {
            
            for (var i = 0; i < data.data.length;) {
                var currentComment = $('#commentInPost_' + postId + data.data[i].id);
                var userNameTag = currentComment.children('.card').children('.card-header').children('#commentUserName');
                var creationDateTag = currentComment.children('.card').children('.card-header').children('#commentCreationDate');
                var textTag = currentComment.children('.card').children('.card-body').children('#commentText');

                currentComment.removeAttr('hidden');
                userNameTag.html(data.data[i].user.userName);
                creationDateTag.html(data.data[i].creationDate.replace('T', ' '));
                textTag.html(data.data[i].text);
                i++;
            }
        }
    })
}

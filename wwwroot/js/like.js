function LikeUnlike(id, entity) {
    var entityId = '#' + id;
    var btn = "";
    var url = "";
    if (entity == "Post") {
        url = '/User/Post/LikeUnlike/';
        btn = $(entityId);
    }
    else {
        url = '/User/Comment/LikeUnlike/';
        btn = $('#comment_' + id);
    }
    var heartIcon = btn.children();
    var likes = heartIcon.text();
    $.ajax({
        type: "POST",
        url: url,
        data: {
            id: id,
        },
        success: function (data) {
            if (data.success == true) {
                likes++;
                btn.html('<i class="fas fa-heart">' + likes);
            }
            else {
                likes--;
                btn.html('<i class="far fa-heart">' + likes);
            }
        }
    })
}

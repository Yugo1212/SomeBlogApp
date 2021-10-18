function LikeUnlike(id) {
    var postId = '#' + id;
    var btn = $(postId);
    var heartIcon = btn.children();
    var likes = heartIcon.text();
    $.ajax({
        type: "POST",
        url: '/User/Like/LikeUnlike/' + id,
        contentType: 'application/json; charset=utf-8',
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

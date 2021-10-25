function LikeUnlike(id) {
    var postId = '#' + id;
    var btn = $(postId);
    var heartIcon = btn.children();
    var likes = heartIcon.text();
    $.ajax({
        type: "POST",
        url: '/User/Like/LikeUnlike/',
        data: {
            id: id,
            entityName: btn.attr('name'),
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

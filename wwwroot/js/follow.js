function FollowUnfollow(id) {
    var userId = '#' + id;
    var btn = $(postId);
    var heartIcon = btn.children();
    var likes = heartIcon.text();
    $.ajax({
        type: "POST",
        url: '/Admin/ApplicationUser/LikeUnlike/',
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

function FollowUnfollow(id) {
    var userId = '#' + id;
    var btn = $(userId);
    var followSpan = btn.children();
    var following = followSpan.children('.value').text();
    $.ajax({
        type: "POST",
        url: '/Admin/ApplicationUser/FollowUnfollow/',
        data: {
            id: id,
        },
        success: function (data) {
            if (data.success == true) {
                following++;
                followSpan.html('Followers:  <span class="value">' + following + '</span> <i class="fas fa-minus-square" style="font-size:1.5rem"></i>');
            }
            else {
                following--;
                followSpan.html('Followers:  <span class="value">' + following + '</span> <i class="fas fa-plus-square" style="font-size:1.5rem">');
            }
        }
    })
}

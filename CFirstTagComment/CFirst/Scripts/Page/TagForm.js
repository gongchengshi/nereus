$(function () {
    $("#submitTag").click(OnAddTagClick);
    $("input[name=newTags]").keydown(function (event) {
        if (event.keyCode == 13) {           
            $("#submitTag").trigger("click");
        }
    });

    $("#submitComment").click(OnAddCommentClick);
    $(".commentText").mousedown(ShowComment);
    $(".commentText").mouseup(HideComment);
});

function OnAddTagClick() {
    var tag = $("input[name=newTags]").val();
    var url = location.href.toString();

    if (tag == "")
    {
        return;
    }
    
    //Add tag to database for user and tag list
    $.ajax({
        url: "/Url/AddTag",
        type: "PUT",
        data: "Name=" + tag + "&Address=" + url

    }).done(function () {
        location.reload(); });
    
}

function OnAddCommentClick() {
    var comment = $("textarea#newComments").val();
    var url = location.href.toString();

    if (comment == "")
    {
        return;
    }

    //Add comment to database for user and url
    $.ajax({
        url: "/Url/AddComment", 
        type: "PUT",
        data: "Content=" +comment + "&Address=" + url

    }).done(function () {
        location.reload();
    });

}
function ShowComment() {
    $(this).removeClass("commentText");
}
function HideComment() {
    $(this).addClass("commentText");
}



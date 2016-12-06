$(function () {
    //Tooltips
    if (tooltipsOn == 'False') {
        $('[title]').each(function () {
            var $this = $(this);
            $this.data('title', $this.attr('title'));
            $this.removeAttr('title');
        });
    }
    if (tooltipsOn == 'True') {
        $('.hasTooltip').tooltip(function () {
            var $this = $(this);
            var title = $this.data('title');
        });
     }
    $("input.tooltipCheckbox").change(OnTooltipClick);


    var str = location.href.toLocaleLowerCase();
    $("#menu li a").each(function () {
        if (str.indexOf(this.href.toLowerCase()) > -1) {
            $("li.pageLink").removeClass("pageLink");
            $(this).parent().addClass("pageLink");
        }
    });

    $("#saveSettings").click(OnOKClick); //Todo:Put in enter key save
});

function OnTooltipClick() {
    var clicked = $(this);
    $.ajax({
        url: "/UserSettings/UpdateUISettings",
        type: "PUT",
        data: "TooltipsOn=" + clicked.is(":checked")        
    });
}
function OnOKClick() {
    var num = $("input#settings-searchResultsNum").val();
    $.ajax({
        url: "/UserSettings/UpdateSearchSettings",
        type: "PUT",
        data: "NumResultsPerPage=" + num
    });      
}

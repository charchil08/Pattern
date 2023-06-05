function formSubmit(callForPagination) {
    event.preventDefault();
    debugger;
    if (!callForPagination) $("#PageIndex").val(1);
    getListAjax();
}

function getListAjax() {
    let formData = $("#skillFilterForm").serialize();
    $.ajax({
        url: "/Skill/GetList",
        method: "POST",
        contentType: "application/x-www-form-urlencoded",
        data: formData,
        success: function (data) {
            debugger;

                $("#SkillListPv").html(data);
                handlePageSize();
                handlePagination();
        },
        error: function (xhr, textStatus, errorThrown) {
            debugger;
            if (xhr.status == 401) {
                location.href = "/Account/Index";
            }
          location.href = "/Account/Index";
        },
        complete: function () {
            debugger;
            setPreserveFilter(formData);
        }
    });
}

$(document).ready(function () {
    $("#pageItem1").addClass("active");

    if (localStorage.getItem("skillFilters") !== null) {
        debugger;
        let filter = localStorage.getItem("skillFilters")
        let params = new URLSearchParams(filter)
        for (let pair of params.entries()) {
            let [key, val] = pair;
            $(`#${key}`).val(val);
        }
    }
    getListAjax();

});

function gotoPage(pageNo) {
    $("#PageIndex").val(pageNo);
    formSubmit(true);
}

function gotoPrevNextPage(isNext) {
    let curPage = Number.parseInt($("#PageIndex").val());
    $("#PageIndex").val(curPage + (isNext ? 1 : -1));
    formSubmit(true);
}

function handlePagination() {
    $(".pagination").children().removeClass("disabled");
    $(".pagination").children().removeClass("active");


    let curPageNo = $("#PageIndex").val();
    $(`#pageItem${curPageNo}`).addClass("active");

    let isFirstPage = Number.parseInt(curPageNo) === 1;
    let isLastPage = $(".pagination").children().length - 2 === Number.parseInt(curPageNo);

    $(".pagination").children().last().toggleClass("disabled", isLastPage);
    $(".pagination").children().first().toggleClass("disabled", isFirstPage);
}

function handlePageSize() {
    $("#pageSizeDropdown").val($("#PageSize").val());
}

function changePageSize() {
    let selectedPageSize = $("#pageSizeDropdown").val();
    $("#PageSize").val(selectedPageSize);
    $("#PageIndex").val(1);
    formSubmit(true);
}

function setPreserveFilter(formData) {
    localStorage.setItem("skillFilters", formData);
}

function getPreserveFilter() {

}
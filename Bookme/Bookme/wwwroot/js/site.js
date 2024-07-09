/*const { post } = require("jquery");*/

function SaveCategoryDetails() {
    debugger
    let name = $("#name").val();

    if (name != "") {
        $.ajax({
            type: 'POST',
            url: '/SuperAdmin/CreateCategory',
            dataType: 'json',
            data:
            {
                categoryName: name,
            },
            success: function (result) {
                debugger
                if (!result.isError) {
                    var url = '/SuperAdmin/Category'; 
                    successAlertWithRedirect(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                errorAlert(result.msg);
            }
        });
    } else {
       
        errorAlert("Invalid, Please fill the form correctly.");
    }
}
function getCategoryDetails(id) {
    $.ajax({
        type: 'GET',
        url: "/SuperAdmin/GetCategoryToEdit",
        data: { categoryId: id },
        success: function (data) {
            $('#categoryId').val(data.data.id);
            $('#categoryName').val(data.data.name);
            $('#editCategory').modal('show');
        }
    });
}
function EditCategoryDetails()
{
    debugger
    let data = {}
    data.id = $("#categoryId").val();
    data.name = $("#categoryName").val();
    var details = JSON.stringify(data);
    if (data.name != "") {
        $.ajax({
            type: 'POST',
            url: '/SuperAdmin/UpdateCategory',
            dataType: 'json',
            data:
            {
                categoryName: details,
            },
            success: function (result) {
                debugger
                if (!result.isError) {
                    var url = '/SuperAdmin/Category';
                    successAlertWithRedirect(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                errorAlert(result.msg);
            }
        });
    } else {
        errorAlert("Invalid, Please fill the form correctly.");
    }
}

function deleteCategoryDetails() {
    debugger
    id = $("#deleteCategoryId").val();
    $.ajax({
        type: 'POST',
        url: '/SuperAdmin/DeleteCategory',
        dataType: 'json',
        data:
        {
            id: id,
        },
        success: function (result) {
            debugger
            if (!result.isError) {
                var url = '/SuperAdmin/Category';
                successAlertWithRedirect(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            errorAlert(result.msg);
        }
    });
    
}
function categoryToDelete(categoryId){
    debugger
    $("#deleteCategoryId").val(categoryId);
}


function userDetails() {
    debugger;
    let data = {}
    /*data.id = $("#applicationUserId").val();*/
    data.FirstName = $("#firsName").val();
    data.LastName = $("#lastName").val();
    data.GenderId = $("#genderId").val();
    data.PhoneNumber = $("#phoneNumber").val();
    data.Address = $("#enterAddress").val();
    data.CategoryId = $("#categoryId").val();
    data.Price = $("#price").val();
    data.UserName = $("#userName").val();
    data.MusicSpecialization = $("#musicSpecialization").val();
    data.Email = $("#email").val();
    data.Password = $("#pwd").val();
    data.ConfirmPassword = $("#confirmpwd").val();
    data.Bio = $("#bio").val();
    if (data.FirstName != "" && data.LastName != "" && data.Gender != "" && data.PhoneNumber != "" && data.Address != "" && data.CategoryId != ""
        && data.Price && data.MusicSpecialization != "" && data.UserName != "" && data.Email != "" && data.Password != "" && data.ConfirmPassword != "" && data.Bio != "") {
        debugger
        var details = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Account/Registration',
            dataType: 'json',
            data:
            {
                registrationDetails: details
            },

             success: function (result) {
                debugger
                if (!result.isError) {
                    var url = '/Account/Login';
                    successAlertWithRedirect(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                errorAlert(result.msg);
            }
        });
    } else {
        debugger
        errorAlert(" Add the required data");
    }
}
function login() {
    debugger;
    Email = $("#email").val();
    Password = $("#password").val();

    if (Email != "" && Password != "") {
        debugger
        $.ajax({
            type: 'POST',
            url: '/Account/login',
            dataType: 'json',
            data:
            {
                email: Email,
                password: Password
            },

            success: function (result) {
                debugger
                if (!result.isError) {
                    location.replace(result.dashboard);
                    /*successAlertWithRedirect(result.msg, url);*/
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                errorAlert(result.msg);
            }
        });
    } else {
        debugger
        errorAlert(" Add the required data");
    }
}

function superAdminRegistration() {
    debugger;
    let data = {}
    data.FirstName = $("#firsname").val();
    data.LastName = $("#lastname").val();
    data.PhoneNumber = $("#phonenumber").val();
    data.UserName = $("#userName").val();
    data.Email = $("#email").val();
    data.Password = $("#pwd").val();
    data.ConfirmPassword = $("#confirmpwd").val();
    if (data.FirstName != "" && data.LastName != "" && data.PhoneNumber != "" && data.UserName != "" && data.Email != "" && data.Password != "" && data.ConfirmPassword != "") {
        debugger
        var details = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Account/SuperAdminRegistration',
            dataType: 'json',
            data:
            {
                superAdminRegistrationDetails: details
            },

            success: function (result) {
                debugger
                if (!result.isError) {
                    var url = '/Account/Login';
                    successAlertWithRedirect(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                errorAlert(result.msg);
            }
        });
    } else {
        debugger
        errorAlert(" Add the required data");
    }

}

function AcceptBooking(id) {
    debugger
    $.ajax({
        type: 'POST',
        url: "/Admin/ApprovedBookings",
        data: { id: id },

        success: function (result) {
            debugger
            if (!result.isError) {
                var url = '/Admin/MyBookings';
                successAlertWithRedirect(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            errorAlert(result.msg);
        }
    });
}

function DeclineBooking(id) {
    debugger
    $.ajax({
        type: 'POST',
        url: "/Admin/Bookings",
        url: "/Admin/DeclineBookings",
        data: { id: id },

        success: function (result) {
            debugger
            if (!result.isError) {
                var url = '/Admin/MyBookings';
                successAlertWithRedirect(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            errorAlert(result.msg);
        }
    });
}

function CancelBooking(id) {
    debugger
    $.ajax({
        type: 'POST',
        url: "/Admin/Bookings",
        url: "/Admin/CancelBookings",
        data: { id: id },

        success: function (result) {
            debugger
            if (!result.isError) {
                var url = '/Admin/MyBookings';
                successAlertWithRedirect(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            errorAlert(result.msg);
        }
    });
}

function getProfileDetails(id) {
    $.ajax({
        type: 'GET',
        url: "/Admin/GetUserProfile",
        data: { userId: id },
        success: function (data) {
            debugger
            $('#userId').val(data.data.id);
            $('#profileFirstName').val(data.data.firstName);
            $('#profileLastName').val(data.data.lastName);
            $('#profileAddress').val(data.data.address);
            $('#profileEmail').val(data.data.email);
            $('#profilePhoneNumber').val(data.data.phoneNumber);
            $('#profileCategory').val(data.data.category.name);
            $('#profilePrice').val(data.data.price);
            $('#profileBio').val(data.data.bio);
            $('#profileMusicSpecialization').val(data.data.musicSpecialization);
            $('#editProfile').modal('show');
        }
    });
}


function EditProfileDetails() {
    debugger
    let data = {}
    data.id = $("#userId").val();
    data.FirstName = $("#profileFirstName").val();
    data.LastName = $("#profileLastName").val();
    data.Address = $("#profileAddress").val();
    data.Email = $("#profileEmail").val();
    data.PhoneNumber = $("#profilePhoneNumber").val();
    data.categoryName = $("#profileCategory").val();
    data.Price = $("#profilePrice").val();
    data.Bio = $("#profileBio").val();
    data.musicSpecialization = $("#profileMusicSpecialization").val();
    var details = JSON.stringify(data);
    if (data.name != "") {
        $.ajax({
            type: 'POST',
            url: '/Admin/UpdateUserProfile',
            dataType: 'json',
            data:
            {
                userDetails: details,
            },
            success: function (result) {
                debugger
                if (!result.isError) {
                    var url = '/Admin/Profile';
                    successAlertWithRedirect(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                errorAlert(result.msg);
            }
        });
    } else {
        errorAlert("Invalid, Please fill the form correctly.");
    }
}


function PostPicture()
{
    debugger
    
    id = $("#userId").val();
    var file = document.getElementById("profilePicture").files;
    if (file[0] != null) {
        const reader = new FileReader();
        reader.readAsDataURL(file[0]);
        reader.onload = function () {
            var base64 = reader.result;
            $.ajax({
                type: 'POST',
                url: '/Admin/UpdatePicture',
                dataType: 'json',
                data:
                {
                    base64: base64,
                    id: id,
                },
                success: function (result) {
                    debugger
                    if (!result.isError) {
                        var url = '/Admin/Profile';
                        successAlertWithRedirect(result.msg, url);
                    }
                    else {
                        errorAlert(result.msg);
                    }
                },
                error: function (ex) {
                    errorAlert(result.msg);
                }
            });
        }
       
    } else {
        errorAlert("Add a picture.");

    }
}

function viewBookedDetails(id) {
    $.ajax({
        type: 'Get',
        url: '/Admin/ViewDetails',
        data:
        {
            id: id,
        },
        success: function (data) {
            $('#userContent').html(data);
            $('#userDetails').show("modal");  
            
        },
    });
}


function available(userId) {
    debugger
    $.ajax({
        type: 'POST',
        url: '/Admin/Available',
        data:
        {
            userId: userId
        },
        success: function (result) {
            debugger
            if (!result.isError) {
                var url = '/Admin/MyBookings';
                successAlertWithRedirect(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            errorAlert(result.msg);
        }
    });
}
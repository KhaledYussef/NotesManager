//-------------------------------------------------------------
function ErrorDialog(title, msg) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: msg,
        showConfirmButton: true,
        showCloseButton: true
    })
}

//-------------------------------------------------------------
function SuccessToast(title, time = 3000) {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        timer: time,
        title: title,
        showConfirmButton: false,
        toast: true,
        showCloseButton: true
    })
}

//---------------------------------------
function dateDiff(date1, date2, lang) {
    var Difference_In_Time = date2.getTime() - date1.getTime();
    var Difference_In_Days = Difference_In_Time / 1000;
    if (lang === 'ar') {
        switch (true) {
            case (Difference_In_Days < 300):
                return 'منذ قليل';
            case ((Difference_In_Days / 60) < 60):
                return `منذ ${Number(Difference_In_Days / 60).toFixed(0)} دقيقة`;
            case (((Difference_In_Days / 60 / 60) >= 1) && ((Difference_In_Days / 60 / 60) < 24)):
                return `منذ ${Number(Difference_In_Days / 60 / 60).toFixed(0)} ساعة`;
            case (((Difference_In_Days / 60 / 60 / 24) >= 1) && ((Difference_In_Days / 60 / 60 / 24) < 30)):
                return `منذ ${Number(Difference_In_Days / 60 / 60 / 24).toFixed(0)} يوم`
            case (((Difference_In_Days / 60 / 60 / 24 / 30) >= 1) && ((Difference_In_Days / 60 / 60 / 24 / 30) < 12)):
                return `منذ ${Number(Difference_In_Days / 60 / 60 / 24 / 30).toFixed(0)} شهر`
            case (Difference_In_Days > 300):
                return Difference_In_Days;
        }
    } else {
        switch (true) {
            case (Difference_In_Days < 300):
                return 'Just Now';
            case ((Difference_In_Days / 60) < 60):
                return `${Number(Difference_In_Days / 60).toFixed(0)} minutes ago`;
            case (((Difference_In_Days / 60 / 60) >= 1) && ((Difference_In_Days / 60 / 60) < 24)):
                return `${Number(Difference_In_Days / 60 / 60).toFixed(0)} hours ago`;
            case (((Difference_In_Days / 60 / 60 / 24) >= 1) && ((Difference_In_Days / 60 / 60 / 24) < 30)):
                return `${Number(Difference_In_Days / 60 / 60 / 24).toFixed(0)} days ago`
            case (((Difference_In_Days / 60 / 60 / 24 / 30) >= 1) && ((Difference_In_Days / 60 / 60 / 24 / 30) < 12)):
                return `${Number(Difference_In_Days / 60 / 60 / 24 / 30).toFixed(0)} months ago`
            case (Difference_In_Days > 300):
                return Difference_In_Days;
        }
    }
}


//-------------------------------------------------------------------
function ConfirmAlert(msg = 'Are You Sure?', func = null) {
    Swal.fire({
        title: msg,
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: 'Yes',
        denyButtonText: `No`,
        icon: 'warning',
    }).then((result) => {
        if (result.isConfirmed) {
            func();
        } else if (result.isDenied) {
            //Swal.fire('Changes are not saved', '', 'info')
        }
    })
}

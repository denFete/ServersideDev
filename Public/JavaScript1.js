function show(target) {

   if( document.getElementById(target).style.display == 'block')
    {
        document.getElementById(target).style.display = 'none';
    }
    else
    {
        document.getElementById(target).style.display = 'block';
    }
}

// skickar en string array till en listbox
function loadAuthor(target)
{

    

   // return document.getElementById(target).innerHTML = '<option>ändrar innerhtml</option>';

}

function load_home() {
    document.getElementById("content").innerHTML = '<object type="text/html" data="~Borrower.cshtml" ></object>';
}

function log_out() {
   
}
function GrabInfo() {
    var str = document.getElementById("apiSearchText").innerHTML;
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "localhost:1819/values/?query=" + str, false);
    xhr.send();
    console.log(xhr.status);
    console.log(xhr.statusText);
    alert(xhr.status);

}
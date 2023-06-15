window.methods = {
    CreateCookie: function (name, value, seconds, path) {
        console.warn(`call -> methods.CreateCookie(name:${name}, value:${value}, seconds:${seconds}, path:${path})`);
        var expires;
        if (seconds) {
            var date = new Date();
            date.setTime(date.getTime() + (seconds * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + `; path=${path}`;
    },
    UpdateCookie: function (name, seconds, path) {
        let value = window.methods.ReadCookie(name);
        console.warn(`call -> methods.UpdateCookie(name:${name}, seconds:${seconds}, path:${path}); set:${value}`);
        window.methods.CreateCookie(name, value, seconds, path);
    },
    ReadCookie: function (cname) {
        console.warn(`call -> methods.ReadCookie(cname:${cname})`);
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },
    ClearStorage: function () {
        localStorage.clear();
        sessionStorage.clear();
    },
    DeleteAllCookies: function () {
        var cookies = document.cookie.split(";");
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
            document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        }
    },
    DeleteCookie: function (name, path, domain) {
        console.warn(`call -> methods.DeleteCookie(name:${name}, path:${path}, domain:${domain})`);
        if (window.methods.ReadCookie(name)) {
            document.cookie = name + "=" +
                ((path) ? ";path=" + path : "") +
                ((domain) ? ";domain=" + domain : "") +
                ";expires=Thu, 01 Jan 1970 00:00:01 GMT";
        }
    }
}
window.onBlazorReady = () => {
    $(document).tooltip({
        track: true
    });
};
window.tooltipeHide = () => {
    $("div.ui-tooltip").hide();
};
window.clipboardCopy = {
    copyText: function (text) {
        parent.navigator.clipboard.writeText(text).then(function () {
            //alert("Copied to clipboard!");
        })
            .catch(function (error) {
                alert(error);
            });
    }
}

window.highlightAll = () => {
    $('pre code').each(function (index_element, object_element) {
        let t = object_element.textContent;
        if (t.substring(0, 1) === '{') {
            object_element.textContent = JSON.stringify(JSON.parse(t), null, 2);
        }
        hljs.highlightBlock(object_element);
    });
}

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    triggerFileDownload(fileName, url);
    URL.revokeObjectURL(url);
}

window.triggerFileDownload = (fileName, url) => {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

window.GetElementHeight = (_id_element) => {
    return document.getElementById(_id_element).offsetHeight;
}

window.GetElementWidth = (_id_element) => {
    return document.getElementById(_id_element).offsetWidth;
}

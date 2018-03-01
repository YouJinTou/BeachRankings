(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = 'https://connect.facebook.net/da_DK/sdk.js#xfbml=1&version=v2.12&appId=1609657982462455&autoLogAppEvents=1';
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
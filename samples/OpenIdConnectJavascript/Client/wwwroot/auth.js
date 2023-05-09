function auth_config() {
    return {
        authority: 'https://localhost:8080',
        client_id: 'js',
        redirect_uri: `${window.location.origin}/callback.html`,
        response_type: 'code',
        scope: 'openid profile api',
    };
}

function auth_check() {
    const mgr = new Oidc.UserManager(auth_config());
    let nonce = Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
    mgr.signinRedirect({
        extraQueryParams: {
            nonce: nonce
        }
    })
}

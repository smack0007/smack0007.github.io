git config --global credential.helper store
git config --global push.default matching
TYPE "%USERPROFILE%\.git-credentials" "https://%1:x-oauth-basic@github.com`n"
cd bin
git push origin
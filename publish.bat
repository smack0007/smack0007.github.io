git config --global credential.helper store
git config --global push.default matching
TYPE "https://%1:x-oauth-basic@github.com`n" > "%USERPROFILE%\.git-credentials"
cd bin
git push origin
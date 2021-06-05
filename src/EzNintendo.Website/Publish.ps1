function New-FtpRequest ($sourceUri, $method, $username, $password) {
    $ftprequest = [System.Net.FtpWebRequest]::Create($sourceuri)
    $ftprequest.Method = $method
    $ftprequest.Credentials = New-Object System.Net.NetworkCredential($username,$password)
    return $ftprequest
}

#Add-FtpFile -ftpFilePath "ftp://myHost.com/folder/somewhere/uploaded.txt" -localFile "C:\temp\file.txt" -userName "User" -password "pw"
function Add-FtpFile($ftpFilePath, $localFile, $username, $password) {
    $ftprequest = New-FtpRequest -sourceUri $ftpFilePath -method ([System.Net.WebRequestMethods+Ftp]::UploadFile) -username $username -password $password
    Write-Host "$($ftpRequest.Method) for '$($ftpRequest.RequestUri)' complete'"
    $content = $content = [System.IO.File]::ReadAllBytes($localFile)
    $ftprequest.ContentLength = $content.Length
    $requestStream = $ftprequest.GetRequestStream()
    $requestStream.Write($content, 0, $content.Length)
    $requestStream.Close()
    $requestStream.Dispose()
}

function Build($output, $configuration) {
    Write-Host "dotnet publish --output $output --framework netcoreapp3.0 --nologo --configuration $configuration"
    dotnet publish --output $output --framework netcoreapp3.0 --nologo --configuration $configuration
}

$outputPath = "$PSScriptRoot\..\.builds\$((Get-Date).ToString('yyyy-MM-ddTHHmmss'))"
$outputFile = "$outputPath.zip"

# Build -output $outputPath -configuration 'Release'
# Write-Host "Compress to $outputFile"
# Compress-Archive -Path $outputPath -CompressionLevel "Optimal" -DestinationPath $outputFile
#UploadToFtp -server "173.249.0.172" -user "orange" -pass "pJ85SQtbhYTPeCmZ4GV3" -path "EzNintendo" -file "E:\source\EzNintendo\src\.builds\2019-10-10T173209.zip" # -file $outputFile
Add-FtpFile -ftpFilePath "ftp://173.249.0.172/EzNintendo/build.zip" -localFile "E:\source\EzNintendo\src\.builds\2019-10-10T173209.zip" -userName "orange" -password "pJ85SQtbhYTPeCmZ4GV3"
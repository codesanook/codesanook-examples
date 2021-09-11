<#
    # Loop to rename (Bash version)
    cp_path=.tmp/${proj_name}/helm-converter/templates/ 
    for get_file in ${cp_path}/*.yaml; do 
        OLDNAME=${get_file##*/}
        NEWNAME=k8s-${0LDNAME}
        mv -v ${cp_path}/${OLDNAME> ${cp_path}/${NEWNAME} 
    done

    # rename 's/^/k8s-/g' *.yaml
#>

# $cp_path = ".tmp/$proj_name/helm-converter/templates/"
$cp_path = "."

# With Foreach-Object
Get-ChildItem $cp_path -Filter *.yaml | Foreach-Object { Rename-Item -Path $_.Fullname -NewName "k8s-$($_.Name)" }

# Only with pipe
Get-ChildItem $cp_path -Filter *.yaml | Rename-Item -NewName { "k8s-$($_.Name)" }

# With alias Cmdlet
gci $cp_path -Filter *.yaml | ren -NewName { "k8s-$($_.Name)" }



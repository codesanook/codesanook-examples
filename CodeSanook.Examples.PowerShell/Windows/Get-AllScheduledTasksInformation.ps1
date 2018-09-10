$tasks = Get-ScheduledTask | ForEach-Object {
    $task = $_

    $triggers = $task.Triggers | ForEach-Object {
        $trigger = $_
        $repetition = @()

        if ($trigger.DaysInterval) {
            $repetition += "DayInterval [$($trigger.DaysInterval)]"
        }

        if ($trigger.WeeksInterval) {
            $repetition += "WeeksInterval [$($trigger.WeeksInterval)]"
        }
        if ($trigger.MonthsInterval) {
            $repetition += "MonthsInterval [$($trigger.MonthsInterval)]"
        }

        if ($trigger.StartBoundary) {
            $repetition += "StartBoundary [$($trigger.StartBoundary )]"
        }

        if($trigger.Repetition -and $trigger.Repetition.Interval){
           $repetition += "Repetition.Interval [$($trigger.Repetition.Interval)]"
        }

        if($repetition.Length){
            "- $($repetition -join ", ")"
        }
    } 

    $actions = $task.Actions | ForEach-Object {
        $action = $_
        "- Execute [$($action.Execute)], Arguments [$($action.Arguments)]"
    } 

    $principal = $task.Principal
    [PSCustomObject]@{
        TaskName = $task.TaskName
        Description = $task.Description
        User     = "GroupId [$($principal.GroupId)], UserId [$($principal.UserId)]"
        Triggers = $triggers -join "`n"
        Actions  = $actions -join "`n"

    }
}

$tasks | Sort-Object -Property TaskName | Format-List
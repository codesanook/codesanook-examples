##Get-ScheduledJob
#https://stackoverflow.com/a/37653546/1872200

Get-ScheduledTask | ForEach-Object {
    $task = $_
    "TaskName: [$($task.TaskName)]"
    if($task.Triggers){
        $task.Triggers | ForEach-Object {
            $trigger = $_
            "DaysInterval        [$($trigger.DaysInterval)]"
            "WeeksInterval       [$($trigger.WeeksInterval)]"
            "MonthsInterval      [$($trigger.MonthsInterval)]"
            "Repetition.Interval [$($trigger.Repetition.Interval)]"
        }
    }
}
using System;

public class Workflow
{
    private StartTaskAction _startTaskAction;
    private SendMessageAction _sendMessageAction;
    private SaveDataAction _saveDataAction;

    public Workflow(int onSendMessageCompleted)
    {
        _startTaskAction = new StartTaskAction();
        _sendMessageAction = new SendMessageAction();
        _saveDataAction = new SaveDataAction();

        _startTaskAction.Completed += OnStartTaskCompleted;
        _sendMessageAction.Completed += OnSendMessageCompleted;
        _saveDataAction.Completed += OnSaveDataCompleted;
        OnSendMessageCompleted = onSendMessageCompleted;
    }

    public int OnSendMessageCompleted { get; }
    public int OnSaveDataCompleted { get; }

    public void Start()
    {
        object value = _startTaskAction.Execute();
    }

    private void OnStartTaskCompleted(object sender, EventArgs e)
    { 

    }
}

internal class SaveDataAction
{
    public SaveDataAction()
    {
    }

    public int Completed { get; internal set; }
}

internal class SendMessageAction
{
    public SendMessageAction()
    {
    }

    public int Completed { get; internal set; }
}

internal class StartTaskAction
{
    public StartTaskAction()
    {
    }

    public Action<object, EventArgs> Completed { get; internal set; }

    internal object Execute()
    {
        throw new NotImplementedException();
    }
}
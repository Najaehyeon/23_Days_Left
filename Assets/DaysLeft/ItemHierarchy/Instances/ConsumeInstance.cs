using UnityEngine;

public class ConsumeInstance : ItemInstance
{
    private ConsumeItemData _consumeItemData;

    public ConsumeInstance(int id) : base(id)
    {
        DataLoadManager dataLoad = Global.Instance.DataLoadManager;

        if (dataLoad == null)
            Debug.LogError("DataLoadManager is not there in Global");
        else
            _consumeItemData = dataLoad.Query(id) as ConsumeItemData;
    }

    public void Consume()
    {
        ActivateEffect();
    }

    private void ActivateEffect()
    {
        _consumeItemData.ConsumeEffect?.Invoke();
    }
}

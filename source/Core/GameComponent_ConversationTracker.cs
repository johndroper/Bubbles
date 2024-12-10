using RimDialogue;
using System.Collections.Generic;
using Verse;
using static System.Net.Mime.MediaTypeNames;
public class GameComponent_ConversationTracker : GameComponent
{
  private Dictionary<string, string> additionalInstructions;
  private List<Conversation> conversations;

  public GameComponent_ConversationTracker(Game game)
  {
    conversations = [];
    additionalInstructions = [];
  }
  public override void FinalizeInit()
  {
    base.FinalizeInit();
    if (conversations == null)
      conversations = [];
    if (additionalInstructions == null)
      additionalInstructions = [];
  }
  public void AddConversation(Pawn initiator, Pawn recipient, string text)
  {
    if (initiator == null || recipient == null || string.IsNullOrWhiteSpace(text))
      return;

    lock(conversations)
    {
      while (conversations.Count > Settings.MaxConversationsStored.Value)
        conversations.RemoveAt(0);
      conversations.Add(new Conversation(initiator, recipient, text));
    }
  }

  public void AddAdditionalInstructions(Pawn pawn, string value)
  {
    lock (additionalInstructions)
    {
      additionalInstructions[pawn?.ThingID ?? "ALL_PAWNS"] = value;
    }
  }

  public string GetAdditionalInstructions(Pawn pawn)
  {
    var thingId = pawn?.ThingID ?? "ALL_PAWNS";
    lock (additionalInstructions)
    {
      if (additionalInstructions.TryGetValue(thingId, out string value))
        return value;
    }
    return string.Empty;
  }

  public List<Conversation> Conversations => conversations;
  public List<Conversation> GetConversationsByPawn(Pawn pawn)
  {
    if (pawn == null)
      return new List<Conversation>();
    lock(conversations)
    {
      return conversations.FindAll(convo => convo.InvolvesPawn(pawn));
    }
  }
  public override void ExposeData()
  {
    base.ExposeData();
    Scribe_Collections.Look(ref conversations, "conversations", LookMode.Deep);
    Scribe_Collections.Look(ref additionalInstructions, "additionalInstructions", LookMode.Value, LookMode.Value);
  }
}

public class Conversation : IExposable
{
  private Pawn initiator;
  private Pawn recipient;
  public string text;

  public Conversation() { }

  public Conversation(Pawn initiator, Pawn recipient, string text)
  {
    this.initiator = initiator;
    this.recipient = recipient;
    this.text = text;
  }
  public Pawn Initiator => initiator;
  public Pawn Recipient => recipient;
  public string Participants => $"{Initiator?.Name?.ToStringShort ?? "Unknown"} ↔ {Recipient?.Name?.ToStringShort ?? "Unknown"}";
  public bool InvolvesPawn(Pawn pawn)
  {
    return pawn.thingIDNumber == initiator.thingIDNumber || pawn.thingIDNumber == recipient.thingIDNumber;
  }
  public void ExposeData()
  {
    Scribe_References.Look(ref initiator, "initiator");
    Scribe_References.Look(ref recipient, "recipient");
    Scribe_Values.Look(ref text, "text");
  }
}
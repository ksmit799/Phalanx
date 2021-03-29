using TaleWorlds.Core;

namespace Phalanx.Util
{
    /**
     * A wrapper around ShowInquiry.
     * Dialogue in this instance is an Inquiry that doesn't accept input.
     * E.g. 'Attempting to join' dialogue.
     */
    public static class DialogueManager
    {
        public static void ShowDialogue(string titleText, string text)
        {
            InformationManager.ShowInquiry(new InquiryData(
                titleText,
                text,
                false,
                false,
                "",
                "",
                null,
                null)
            );
        }
    }
}
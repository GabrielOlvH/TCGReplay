namespace compiler
{
    public enum TokenType
    {
        // Players
        BluePlayer,
        RedPlayer,
        
        //Actions
        Drew,
        Played,
        Attach,
        Shuffled,
        Put,
        Discarded,
        Evolved,
        Retreated,
        KnockedOut,
        AddedPrize,
        Switched,
        Used,
        Moved,
        Round,
        
        // Locations
        Location,
        
        // Additions
        FollowUp,
        And,
        Into,
        To,
        From,
        IsNow,
        SequenceSplitter,
        
        // EOL
        Invalid,
        SequenceTerminator,
        NewLine,
        Ignored,
        
        // Types
        Number,
    }
}
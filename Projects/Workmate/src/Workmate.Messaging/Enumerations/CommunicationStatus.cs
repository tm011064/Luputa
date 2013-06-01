namespace Workmate.Messaging
{
  /// <summary>
  /// Defines the states in which an System.ServiceModel.ICommunicationObject can exist.
  /// </summary>
  public enum CommunicationStatus
  {
    /// <summary>Indicates that the communication object has been instantiated and is configurable,
    /// but not yet open or ready for use.
    /// </summary>
    Created = 0,
    /// <summary>
    /// Indicates that the communication object is being transitioned from the System.ServiceModel.CommunicationState.Created
    /// state to the System.ServiceModel.CommunicationState.Opened state.
    /// </summary>
    Opening = 1,
    /// <summary>Indicates that the communication object is now open and ready to be used.
    /// </summary>
    Opened = 2,
    /// <summary>
    /// Indicates that the communication object is transitioning to the System.ServiceModel.CommunicationState.Closed
    /// state.
    /// </summary>
    Closing = 3,
    /// <summary>
    /// Indicates that the communication object has been closed and is no longer
    /// usable.
    /// </summary>
    Closed = 4,
    /// <summary>
    /// Indicates that the communication object has encountered an error or fault
    /// from which it cannot recover and from which it is no longer usable.
    /// </summary>
    Faulted = 5,
  }
}

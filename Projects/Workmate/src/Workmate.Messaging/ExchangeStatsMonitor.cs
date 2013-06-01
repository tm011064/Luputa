using System;
using System.Threading;
using log4net;

namespace Workmate.Messaging
{
  /// <summary>
  /// 
  /// </summary>
  public class ExchangeMessageMonitor : IDisposable
  {
    #region nested classes
    class ExchangeStatsSnapshot
    {
      public long TotalMessagesSent { get; set; }
      public long TotalMessagesSentDuringLastSnapshotInterval { get; set; }
      public TimeSpan LastSnapshotInterval { get; set; }
      public DateTime DateCreatedUtc { get; set; }

      public override string ToString()
      {
        int paddingLeft = 12;
        int paddingRight = 42;

        return
  @"Exchange Stats:
  " + "Total messages sent".PadRight(paddingRight) + " " + this.TotalMessagesSent.ToString().PadLeft(paddingLeft) + @"
  " + ("Total messages sent in last " + LastSnapshotInterval.TotalSeconds + " seconds").PadRight(paddingRight) + " " + this.TotalMessagesSentDuringLastSnapshotInterval.ToString().PadLeft(paddingLeft) + @"
  " + "Average messages sent during last interval".PadRight(paddingRight) + " " + ((double)this.TotalMessagesSentDuringLastSnapshotInterval / this.LastSnapshotInterval.TotalSeconds).ToString("N2").PadLeft(paddingLeft) + @" msg/s
";
      }

      public ExchangeStatsSnapshot(long totalMessagesSent, long totalMessagesSentDuringLastSnapshotInterval, TimeSpan lastSnapshotInterval)
      {
        this.TotalMessagesSent = totalMessagesSent;
        this.TotalMessagesSentDuringLastSnapshotInterval = totalMessagesSentDuringLastSnapshotInterval;
        this.LastSnapshotInterval = lastSnapshotInterval;

        this.DateCreatedUtc = DateTime.UtcNow;
      }
    }
    #endregion

    #region members
    private ILog _Log = LogManager.GetLogger("ExchangeMessageMonitor");

    private ExchangeStatsSnapshot _LastExchangeStatsSnapshot;
    private Timer _SnapshotTimer;
    private TimeSpan _SnapshotInterval;
    private Exchange _Exchange;
    #endregion

    #region private methods
    private void TakeSnapshot(object state)
    {
      _SnapshotTimer.Change(Timeout.Infinite, Timeout.Infinite);

      ExchangeStatsSnapshot lastExchangeStatsSnapshot = _LastExchangeStatsSnapshot;

      long lastTotalMessagesSent;
      if (lastExchangeStatsSnapshot != null)
        lastTotalMessagesSent = lastExchangeStatsSnapshot.TotalMessagesSent;
      else
        lastTotalMessagesSent = 0;

      long totalMessagesSent = _Exchange.TotalMessagesSent;

      _LastExchangeStatsSnapshot = new ExchangeStatsSnapshot(totalMessagesSent, totalMessagesSent - lastTotalMessagesSent, _SnapshotInterval);
      _Log.Info(_LastExchangeStatsSnapshot.ToString());

      _SnapshotTimer.Change(_SnapshotInterval, _SnapshotInterval);
    }
    #endregion

    #region public methods
    /// <summary>
    /// Starts the snapshot logging.
    /// </summary>
    public void StartSnapshotLogging()
    {
      if (_SnapshotTimer != null)
      {
        _SnapshotTimer.Dispose();
        _SnapshotTimer = new Timer(TakeSnapshot);
      }

      _SnapshotTimer.Change(_SnapshotInterval, _SnapshotInterval);
    }

    /// <summary>
    /// Stops the snapshot logging.
    /// </summary>
    public void StopSnapshotLogging()
    {
      _SnapshotTimer.Dispose();
    }
    #endregion

    #region IDisposable Members

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      if (_SnapshotTimer != null)
        _SnapshotTimer.Dispose();
    }

    #endregion

    #region constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeMessageMonitor"/> class.
    /// </summary>
    /// <param name="snapshotInterval">The snapshot interval.</param>
    public ExchangeMessageMonitor(TimeSpan snapshotInterval, Exchange exchange)
    {
      _Exchange = exchange;
      _SnapshotInterval = snapshotInterval;
      _SnapshotTimer = new Timer(TakeSnapshot);
    }
    #endregion
  }
}

/*  CTRADER GURU --> Indicator Template 1.0.6

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;

namespace cAlgo
{

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MegaChannel : Indicator
    {

        #region Enums

        // --> Eventuali enumeratori li mettiamo qui

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Mega Channel";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.3";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/indicator-base/")]
        public string ProductInfo { get; set; }

        [Parameter("MA Type", Group = "Params", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MaType { get; set; }

        [Parameter("Period", Group = "Params", DefaultValue = 14)]
        public int Period { get; set; }

        [Parameter("Smoothed", Group = "Params", DefaultValue = 30)]
        public int Smoothed { get; set; }

        [Parameter("Adjustment", Group = "Params", DefaultValue = 1.8)]
        public double Adjust { get; set; }

        [Output("Top Channel", LineColor = "#b1cbbb", PlotType = PlotType.Line)]
        public IndicatorDataSeries Tchannel { get; set; }

        [Output("Mid Channel", LineColor = "#000000", PlotType = PlotType.Line, LineStyle = LineStyle.DotsRare)]
        public IndicatorDataSeries Mchannel { get; set; }

        [Output("Bottom Channel", LineColor = "#c94c4c", PlotType = PlotType.Line)]
        public IndicatorDataSeries Bchannel { get; set; }

        #endregion

        #region Property

        private MovingAverage _MyMAup;
        private MovingAverage _MyMA;
        private MovingAverage _MyMAdw;

        private DirectionalMovementSystem _MyADX;

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            _MyMAup = Indicators.MovingAverage(Bars.HighPrices, Period, MaType);
            _MyMA = Indicators.MovingAverage(Bars.ClosePrices, Period, MaType);
            _MyMAdw = Indicators.MovingAverage(Bars.LowPrices, Period, MaType);

            _MyADX = Indicators.DirectionalMovementSystem(Smoothed);

        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            double k = (_MyADX.ADX.LastValue * Symbol.PipSize) * Adjust;

            Tchannel[index] = _MyMAup.Result.LastValue + k;
            Mchannel[index] = _MyMA.Result.LastValue;
            Bchannel[index] = _MyMAdw.Result.LastValue - k;

        }

        #endregion

        #region Private Methods

        // --> Seguiamo la signature con underscore "_mioMetodo()"

        #endregion

    }

}
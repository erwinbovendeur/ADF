using System.Web.UI.WebControls;

namespace Adf.Web
{
    /// <summary>
    /// Defines methods that a grid service class implements to do the activities at the 
    /// time of initiation and handling of the service for a <see cref="GridView"/>".
    /// </summary>
    public interface IGridService
    {
        /// <summary>
        /// Does the activities required at the time of initiation of the grid service for the
        /// specified <see cref="GridView"/>.
        /// </summary>
        /// <param name="view">The <see cref="GridView"/> for which 
        /// the activities to be done.</param>
        /// <param name="p">The parameters used to do the activities.</param>
        void InitService(GridView view, params object[] p);

        /// <summary>
        /// Handles the activities for the specified <see cref="GridView"/>.
        /// </summary>
        /// <param name="action">The <see cref="GridAction"/> to perform.</param>
        /// <param name="view">The <see cref="GridView"/> for which the 
        /// activities to be handled.</param>
        /// <param name="p">The parameters used to handle the activities.</param>
        void HandleService(GridAction action, GridView view, params object[] p);
    }
}

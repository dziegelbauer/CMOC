import {Link} from "react-router-dom";
import {ShieldFillExclamation, ShieldFillCheck, ShieldFillX} from "react-bootstrap-icons";

type Args = {
    status: number;
    title: string;
    id: number;
    drillDownPath: string;
}

const StatusCard = ({status, title, id, drillDownPath}: Args) => {
    let cardStyle;
    let titleText;
    
    switch(status)
    {
        case 0:
            cardStyle = "card h-100 text-bg-success";
            titleText = <><ShieldFillCheck/>{" " + title}</>
            break;
        case 1:
            cardStyle = "card h-100 text-bg-warning";
            titleText = <><ShieldFillExclamation/>{" " + title}</>
            break;
        default:
            cardStyle = "card h-100 text-bg-danger";
            titleText = <><ShieldFillX/>{" " + title}</>
            break;
    }
    
    return (
        <div className="col">
            <div className={cardStyle}>
                <div className="card-body">
                    <h5 className="card-title">
                        <Link to={`${drillDownPath}/${id}`} className="text-decoration-none text-white">
                            {titleText}
                        </Link>
                    </h5>
                    <p className="card-text">This is some placeholder text.</p>
                </div>
                <div className="card-footer">
                    <small className="text-body-secondary">Last updated 3 minutes ago</small>
                </div>
            </div>
        </div>
    )
}

export default StatusCard;
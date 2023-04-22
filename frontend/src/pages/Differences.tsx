import {Title} from "../components/Title";
import {GetMaxDifference} from "../api/Api";

function Differences() {
    GetMaxDifference("usd", 255);
    return (
        <div>
            <Title value="Differences"/>
        </div>
    )
}

export {Differences};
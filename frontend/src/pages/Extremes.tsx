import {Title} from "../components/Title";
import {GetExtremeExchangeRates} from "../api/Api";

function Extremes() {
    GetExtremeExchangeRates("eur", 255);
    return (
        <div>
            <Title value="Extremes"/>
        </div>
    )
}

export {Extremes};
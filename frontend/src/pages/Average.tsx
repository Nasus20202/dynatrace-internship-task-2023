import {Title} from "../components/Title";
import {GetAverageExchangeRate} from "../api/Api";

function Average() {
    let date = new Date(2022, 1,1); let code = "krw";
    GetAverageExchangeRate(code, date);
    return (
        <div>
            <Title value="Average"/>
        </div>
    )
}

export {Average};
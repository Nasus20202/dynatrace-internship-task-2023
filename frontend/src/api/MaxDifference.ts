import apiPath from './ApiPath'

type MaxDifference = {
    difference?: number,
    date?: Date,
    statusCode: number,
    message?: string
}

export default async function GetMaxDifference(code: string, quotations: number) : Promise<MaxDifference> {
    const path = `${apiPath}/exchange/maxBuyAskDifference/${code}/${quotations}`;
    const response = await fetch(path);
    if(response.status !== 200) {
        return {statusCode: response.status, message: await response.text()};
    }
    const data = await response.json();
    return {difference: data.maxDifference.value, date: data.maxDifference.date, statusCode: response.status};
}
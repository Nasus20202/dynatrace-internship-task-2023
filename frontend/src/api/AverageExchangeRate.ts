import apiPath from './ApiPath'

type AverageExchangeRate = {
    rate?: number,
    statusCode: number,
    message?: string
}

export default async function GetAverageExchangeRate(code: string, date: Date) : Promise<AverageExchangeRate>{
    const path = `${apiPath}/exchange/average/${code}/${date.getFullYear()}-${date.getMonth()+1}-${date.getDate()}`;
    const response = await fetch(path);
    if(response.status !== 200)
        return {statusCode: response.status, message: await response.text()};

    const data = await response.json();
    return {rate: data.exchangeRate, statusCode: response.status};
}



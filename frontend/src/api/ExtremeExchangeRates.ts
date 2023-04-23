import apiPath from './ApiPath'

type ExtremeExchangeRates = {
    minRate?: number,
    minDate?: Date,
    maxRate?: number,
    maxDate?: Date,
    statusCode: number,
    message?: string
}

export default async function GetExtremeExchangeRates(code: string, quotations: number) : Promise<ExtremeExchangeRates>{
    const path = `${apiPath}/exchange/extremes/${code}/${quotations}`;
    const response = await fetch(path);
    if(response.status !== 200) {
        return {statusCode: response.status, message: await response.text()};
    }
    const data = await response.json();
    return {minRate: data.minExchangeRate.value, minDate: data.minExchangeRate.date,
        maxRate: data.maxExchangeRate.value, maxDate: data.maxExchangeRate.date,
        statusCode: response.status};
}

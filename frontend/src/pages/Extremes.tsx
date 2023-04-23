import {Title} from "../components/Title";
import React from "react";
import {Form, Button, Row, Col, Alert} from "react-bootstrap";
import GetExtremeExchangeRates from "../api/ExtremeExchangeRates";

interface IProps {
}

interface IState {
    currencyCode: string;
    currencyCodeInput: string;
    quotations : number;
    quotationsInput : number;
    minDate: Date;
    minResult: number;
    maxDate: Date;
    maxResult: number;
    waiting: boolean;
    message: string;
}

class Extremes extends React.Component<IProps, IState> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            currencyCode: "",
            currencyCodeInput: "",
            quotations: 0,
            quotationsInput: 0,
            minDate: new Date(),
            minResult: -1,
            maxDate: new Date(),
            maxResult: -1,
            waiting: false,
            message: "",
        };

        this.handleCurrencyCodeChange = this.handleCurrencyCodeChange.bind(this);
        this.handleQuotationsChange = this.handleQuotationsChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleCurrencyCodeChange = (event: any) => {
        this.setState({
            currencyCodeInput: event.target.value.toUpperCase(),
        });
    }

    handleQuotationsChange = (event: any) => {
        this.setState({
            quotationsInput: event.target.value
        });
    }

    handleSubmit = (event: any) => {
        event.preventDefault();
        if(!this.validateForm())
            return;
        this.setState({
            waiting: true,
            currencyCode: this.state.currencyCodeInput,
            quotations: this.state.quotationsInput
        });
        GetExtremeExchangeRates(this.state.currencyCodeInput, this.state.quotationsInput).then(response => {
            this.setState({
                waiting: false,
                minResult: response.minRate !== undefined ? response.minRate as number : -1,
                minDate: response.minDate !== undefined ? response.minDate as Date : new Date(),
                maxResult: response.maxRate !== undefined ? response.maxRate as number : -1,
                maxDate: response.maxDate !== undefined ? response.maxDate as Date : new Date(),
                message: response.message !== undefined ? response.message as string : ""
            });
        });
    }

    validateForm() : boolean{
        if(this.state.currencyCodeInput.length !== 3) {
            this.setState({
                message: "Currency code must be 3 characters long"
            });
            return false;
        }
        if(this.state.quotationsInput > 255 || this.state.quotationsInput < 1) {
            this.setState({
                message: "Invalid number of quotations (1-255)"
            });
            return false;
        }
        this.setState({
            message: ""
        })
        return true;
    }

    render() {
        return (
            <div>
                <Title value="Extremes"/>
                <Form onSubmit={this.handleSubmit}>
                    <Form.Group as={Row} className="mb-3">
                        <Form.Label column sm="2">
                            Currency code:
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control type="text" placeholder="Enter currency code" name="currencyCode" onChange={this.handleCurrencyCodeChange} disabled={this.state.waiting}/>
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} className="mb-3">
                        <Form.Label column sm="2">
                            Quotations:
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control type="number" placeholder="Enter quotations" name="date" onChange={this.handleQuotationsChange} disabled={this.state.waiting}/>
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} className="mb-3">
                        <Col sm={{span: 10, offset: 2}}>
                            <Button type="submit" disabled={this.state.waiting}>Submit</Button>
                        </Col>
                    </Form.Group>
                </Form>
                <Alert variant="danger" show={this.state.message.length !== 0}>{this.state.message}</Alert>
                <Alert variant="success" show={this.state.minResult !== -1}>
                    Min exchange rate for {this.state.currencyCode} in last {this.state.quotations} quotations is {this.state.minResult} zł ({this.state.minDate.toString()})<br/>
                    Max exchange rate for {this.state.currencyCode} in last {this.state.quotations} quotations is {this.state.maxResult} zł ({this.state.maxDate.toString()})
                </Alert>
            </div>
        );
    }
}

export {Extremes};
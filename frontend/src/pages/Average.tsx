import {Title} from "../components/Title";
import {GetAverageExchangeRate} from "../api/Api";
import React from "react";
import {Form, Button, Row, Col, Alert} from "react-bootstrap";

interface IProps {
}

interface IState {
    currencyCode: string;
    date: string;
    result: number;
    waiting: boolean;
    message: string;
}

class Average extends React.Component<IProps, IState> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            currencyCode: "",
            date: "",
            result: 0,
            waiting: false,
            message: ""
        };

        this.handleCurrencyCodeChange = this.handleCurrencyCodeChange.bind(this);
        this.handleDateChange = this.handleDateChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleCurrencyCodeChange = (event: any) => {
        this.setState({
            currencyCode: event.target.value
        });
    }

    handleDateChange = (event: any) => {
        this.setState({
            date: event.target.value
        });
    }

    handleSubmit = (event: any) => {
        event.preventDefault();
        if(!this.validateForm())
            return;
        this.setState({
            waiting: true
        });
        let date = new Date(this.state.date);
        GetAverageExchangeRate(this.state.currencyCode, date).then(response => {
            this.setState({
                result: response.rate,
                waiting: false
            });
        });
    }

    validateForm() : boolean{
        if(this.state.currencyCode.length !== 3) {
            this.setState({
                message: "Currency code must be 3 characters long"
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
            <Title value="Average"/>
            <Form onSubmit={this.handleSubmit}>
                <Form.Group as={Row} className="mb-3">
                    <Form.Label column sm="2">
                        Currency code:
                    </Form.Label>
                    <Col sm="10">
                        <Form.Control type="text" placeholder="Enter currency code" name="currencyCode" onChange={this.handleCurrencyCodeChange}/>
                    </Col>
                </Form.Group>
                <Form.Group as={Row} className="mb-3">
                    <Form.Label column sm="2">
                        Date:
                    </Form.Label>
                    <Col sm="10">
                        <Form.Control type="date" placeholder="Enter date" name="date" onChange={this.handleDateChange}/>
                    </Col>
                </Form.Group>
                <Form.Group as={Row} className="mb-3">
                    <Col sm={{span: 10, offset: 2}}>
                        <Button type="submit">Submit</Button>
                    </Col>
                </Form.Group>
            </Form>
            <Alert variant="danger" show={this.state.message.length !== 0}>{this.state.message}</Alert>
            <Alert variant="success" show={this.state.result !== 0}>Average exchange rate for {this.state.currencyCode} on {this.state.date} is {this.state.result}</Alert>
        </div>
        );
    }
}

export {Average};
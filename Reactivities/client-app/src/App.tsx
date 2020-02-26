import React, { Component } from "react";
import { Header, Icon, List } from "semantic-ui-react";
import "./App.css";
import axios from "axios";

class App extends Component {
  state = {
    Values: []
  };

  componentDidMount() {
    axios.get("http://localhost:5000/weatherforecast").then(response => {
      this.setState({
        Values: response.data
      });
    });
  }
  render() {
    return (
      <div>
        <Header as="h2">
          <Icon name="users" />
          <Header.Content>Reactivities</Header.Content>
        </Header>

        <List>
          {this.state.Values.map((res: any) => (
            <List.Item key={res.id}>{res.name}</List.Item>
          ))}
        </List>
      </div>
    );
  }
}

export default App;

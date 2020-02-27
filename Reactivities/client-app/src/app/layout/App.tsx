import React, { Component } from "react";
import { Header, Icon, List } from "semantic-ui-react";
import axios from "axios";

class App extends Component {
  state = {
    activities: []
  };

  componentDidMount() {
    axios.get("http://localhost:5000/api/activities").then(response => {
      this.setState({
        activities: response.data
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
          {this.state.activities.map((res: any) => (
            <List.Item key={res.id}>{res.name}</List.Item>
          ))}
        </List>
      </div>
    );
  }
}

export default App;

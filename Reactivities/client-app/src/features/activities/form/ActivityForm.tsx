import React, {useState} from "react";
import { Segment, Form, Button } from "semantic-ui-react";
import { IActivity } from "../../../app/models/activity";

interface IProps{
    setEditMode: (edit:boolean)=> void
    activity: IActivity
}
export const ActivityForm:React.FC<IProps> = ({ setEditMode, activity: initialFormState}) => {

    const initializeForm = ()=> {
        if (initialFormState) {
            return initialFormState;
        }
        else {
           return {
               id: "",
               title: "",
               category: "",
               description: "",
               date: "",
               city: "",
               venue: ""
           }
        }
    };

    const [activity, setActivity] = useState<IActivity>(initializeForm)

  return (
    <Segment clearing>
      <Form>
        <Form.Input placeholder="Title" vaue={activity.title} />
        <Form.TextArea row={2} placeholder="Description" value={activity.description} />
        <Form.Input placeholder="Category" value={activity.category} /> 
        <Form.Input type='date' placeholder="Date" value={activity.date} />
              <Form.Input placeholder="City" value={activity.city} />
              <Form.Input placeholder="Venue" value={activity.venue} />
        <Button floated='right' positive type='submit' content='Submit'/>
        <Button onClick={()=> setEditMode(false)} floated='right' type='button' content='Cancel' />
      </Form>
    </Segment>
  );
};

import React from 'react'
import { Item, Button, Segment, Label } from 'semantic-ui-react'
import { IActivity } from '../../../app/models/activity'

interface IProps {
    activities: IActivity[];
    selectActivity: (id:string)=> void
}

export const ActivityList: React.FC<IProps> = ({ activities, selectActivity}) => {
    return (
    <Segment clearing>
            <Item.Group divided>
                {activities.map(activity=>(

                    <Item key={activity.id}>
                        <Item.Content>
                <Item.Header as='a'>{activity.title}</Item.Header>
                <Item.Meta>{activity.date}</Item.Meta>
                            <Item.Description>
                <div>{activity.description}</div>
                <div>{activity.city}, {activity.venue}</div>
                                {/* <Image src='https://react.semantic-ui.com/images/wireframe/short-paragraph.png' /> */}
                            </Item.Description>
                            <Item.Extra>
                                <Button floated='right' content='View' color='blue' onClick={()=> selectActivity(activity.id)} />
                                <Label basic content={activity.category} />
                            </Item.Extra>
                        </Item.Content>
                    </Item>
                ))}
        
            </Item.Group>
    </Segment>
    )
}

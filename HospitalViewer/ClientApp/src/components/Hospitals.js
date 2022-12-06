import React, { Component } from 'react';
import parse from 'html-react-parser'

export class Hospitals extends Component {
  static displayName = Hospitals.name;

  constructor(props) {
    super(props);
    this.state = { hospitals: [], loading: true };
  }

  componentDidMount() {
    this.populateHospitalData();
  }

  static renderHospitalTable(hospitals) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Address</th>
            <th>Phone Number</th>
            <th><input type="button">Add</input></th>
          </tr>
        </thead>
        <tbody>
          {hospitals.map(hospital =>
            <tr key={hospital.HospitalId}>
              <td>{hospital.name}</td>
              <td>{hospital.description}</td>
              <td>{parse(hospital.addressDisplayHtml)}</td>
              <td>{hospital.phoneNumber}</td>
              <td><input type="button">Contacts</input><br/><input type="button">Edit</input></td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Hospitals.renderHospitalTable(this.state.hospitals);

    return (
      <div>
        <h1 id="tabelLabel">Hospitals</h1>
        {contents}
      </div>
    );
  }

  async populateHospitalData() {
    //const token = await authService.getAccessToken();
    const response = await fetch('api/hospitals', {
      //headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ hospitals: data, loading: false });
  }
}
